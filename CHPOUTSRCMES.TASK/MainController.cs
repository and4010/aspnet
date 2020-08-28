using CHPOUTSRCMES.TASK.Forms;
using CHPOUTSRCMES.TASK.Models;
using CHPOUTSRCMES.TASK.Models.Entity;
using CHPOUTSRCMES.TASK.Models.Entity.Temp;
using CHPOUTSRCMES.TASK.Models.UnitOfWork;
using CHPOUTSRCMES.TASK.Models.Views;
using CHPOUTSRCMES.TASK.Tasks;
using CHPOUTSRCMES.TASK.Tasks.Interfaces;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHPOUTSRCMES.TASK
{
    public class MainController : IDisposable
    {

        private static MainController instance;

        internal static MainController Instance => instance ?? (instance = new MainController());

        private Logger logger = LogManager.GetCurrentClassLogger();

        private String erpConnStr;

        internal String ErpConnStr => erpConnStr ?? (erpConnStr =
#if DEBUG
            System.Configuration.ConfigurationManager.ConnectionStrings["OracleTestContext"].ToString()
#else
            System.Configuration.ConfigurationManager.ConnectionStrings["ErpContext"].ToString()
#endif
            );

        private String mesConnStr;

        internal String MesConnStr => mesConnStr ?? (mesConnStr =
#if DEBUG
            System.Configuration.ConfigurationManager.ConnectionStrings["MesTestContext"].ToString()
#else
            System.Configuration.ConfigurationManager.ConnectionStrings["MesContext"].ToString()
#endif
            );

        private bool disposed = false;
        /// <summary>
        /// 取消權杖來源
        /// </summary>
        private CancellationTokenSource cancelTokenSource { set; get; }
        /// <summary>
        /// 取消權杖
        /// </summary>
        private CancellationToken cancelToken { set; get; }
        /// <summary>
        /// 任務清單
        /// </summary>
        private List<ITasker> taskers { set; get; }

        private Dictionary<string, Task> taskList { set; get; }

        private LimitedConcurrencyLevelTaskScheduler taskScheduler { set; get; }

        private TaskFactory factory { set; get; }

        private MainForm mainForm = null;

        internal MainForm MainForm => mainForm ?? (mainForm = new MainForm() { Controller = this });

        private UomConverterForm uomConverterForm = null;

        internal UomConverterForm UomConverterForm => (uomConverterForm == null || uomConverterForm.IsDisposed) ? (uomConverterForm = new UomConverterForm() { Controller = this }) : uomConverterForm;

        private MasterViewForm masterViewForm = null;

        internal MasterViewForm MasterViewForm => (masterViewForm == null || masterViewForm.IsDisposed) ? (masterViewForm = new MasterViewForm() { Controller = this }) : masterViewForm;

        /// <summary>
        /// 
        /// </summary>
        public MainController()
        {
            taskScheduler = new LimitedConcurrencyLevelTaskScheduler(20);
            factory = new TaskFactory(taskScheduler);
            taskers = new List<ITasker>();
            taskList = new Dictionary<string, Task>();
        }

        #region 任務計數器

        /// <summary>
        /// 開始計時器
        /// </summary>
        internal void StartTimer()
        {
            cancelTokenSource = new CancellationTokenSource();
            cancelToken = cancelTokenSource.Token;
            var date = DateTime.Now.AddMinutes(1); //開始時間加一分鐘為啟動時間
            //Timer 一分鐘時間啟動一次
            Tasks.TaskTimer.Instance.Schedule(date.Hour, date.Minute, 1,
                () =>
            {


                foreach (ITasker profile in taskers)
                {
                    if (taskList.ContainsKey(profile.Name))
                        continue;

                    if (cancelToken.IsCancellationRequested)
                    {
                        cancelToken.ThrowIfCancellationRequested();
                    }

                    try
                    {
                        var task = profile.Run(factory, cancelToken, (n, t) =>
                        {
                            if (taskList.ContainsKey(n))
                            {
                                taskList.Remove(n);
                            }

                        });

                        if (task != null)
                        {
                            taskList.Add(profile.Name, task);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
            });
        }

        /// <summary>
        /// 停止計時器
        /// </summary>
        internal void StopTimer()
        {
            if (!cancelTokenSource.IsCancellationRequested)
            {
                cancelTokenSource.Cancel();
            }

            try
            {
                Task.WaitAll(taskList.Values.ToArray());
            }
            catch (OperationCanceledException)
            {
                LogInfo("使用者取消 OperationCanceledException");
            }
            finally
            {
                cancelTokenSource.Dispose();
            }
        }

        /// <summary>
        /// 加入 TASKER
        /// </summary>
        /// <param name="tasker"></param>
        internal void AddTasker(ITasker tasker)
        {
            if(!TaskerExists(tasker.Name))
            {
                taskers.Add(tasker);
            }
        }

        internal bool TaskerExists(string taskname)
        {
            bool found = false;
            for (int i = 0; i < taskers.Count; i++)
            {
                string name = taskers?[i]?.Name;
                found = !string.IsNullOrEmpty(name) && taskname.CompareTo(name) == 0;
                if (found) break;
            }
            return found;
        }

        #endregion 任務計數器

        #region 主檔更新 (ORACLE VIEWs TO MES)

        internal void AddMasterTasker()
        {
            AddTasker(new Tasker("主檔轉檔程序", 1, (tasker, token) => {
                //組織倉庫儲位
                var taskSubinventory = importSubinventory(tasker, token);
                //庫存交易類別
                var taskTransactionType = importTransactionType(tasker, token);
                //料號
                var taskItem = taskSubinventory.ContinueWith(task => importItem(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //餘切規格
                var taskOspRelatedItem = taskItem.ContinueWith(subTask => importOspRelatedItem(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //紙別機台
                var taskMachinePaperType = taskItem.ContinueWith(subTask => importMachinePaperType(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //令重張數
                var tasYszmpckq = taskItem.ContinueWith(subTask => importYszmpckq(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(taskSubinventory, taskTransactionType, taskItem, taskOspRelatedItem, taskMachinePaperType, tasYszmpckq);

                AddCtrStTasker();
            }));
        }

        /// <summary>
        /// 同步 作業單元、組織、倉庫、儲位 資料表
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task importSubinventory(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_SUBINVENTORY_V> list = new List<XXCINV_SUBINVENTORY_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        list = (await masterUOW.GetSubinventoryListAsync()).ToList();

                        if (list.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-無法從 {typeof(XXCINV_SUBINVENTORY_V)}VIEW取得資料");
                            return;
                        }
                    }
                }

                var orgUnitList = list
                    .GroupBy(x => x.ORG_ID)
                    .Select(x => new ORG_UNIT_TMP_T()
                    {
                        ORG_ID = x.FirstOrDefault().ORG_ID,
                        ORG_NAME = x.FirstOrDefault().ORG_NAME,
                        CONTROL_FLAG = ""
                    })
                    .ToList();

                //取得組織
                var organizationList = list
                    .GroupBy(x => x.ORGANIZATION_ID)
                    .Select(x => new ORGANIZATION_TMP_T() {
                        ORG_ID = x.FirstOrDefault().ORG_ID,
                        ORGANIZATION_ID = x.FirstOrDefault().ORGANIZATION_ID,
                        ORGANIZATION_NAME = x.FirstOrDefault().ORGANIZATION_NAME,
                        ORGANIZATION_CODE = x.FirstOrDefault().ORGANIZATION_CODE,
                        CONTROL_FLAG = ""
                    })
                    .ToList();

                //取得倉庫
                var subList = list
                    .GroupBy(x => new { x.ORGANIZATION_ID, x.SUBINVENTORY_CODE })
                    .Select(x => new SUBINVENTORY_TMP_T()
                    {
                        ORGANIZATION_ID = x.FirstOrDefault().ORGANIZATION_ID,
                        SUBINVENTORY_CODE = x.FirstOrDefault().SUBINVENTORY_CODE,
                        SUBINVENTORY_NAME = x.FirstOrDefault().SUBINVENTORY_NAME,
                        LOCATOR_TYPE = x.FirstOrDefault().LOCATOR_TYPE,
                        OSP_FLAG = x.FirstOrDefault().OSP_FLAG,
                        CONTROL_FLAG = ""
                    })
                    .ToList();
                //取得儲位
                var locatorList = list
                    .Where(x => x.LOCATOR_ID != null && x.LOCATOR_ID > 0)
                    .GroupBy(x => new { x.ORGANIZATION_ID, x.SUBINVENTORY_CODE, x.LOCATOR_ID })
                    .Select(x => new LOCATOR_TMP_T()
                    {
                        ORGANIZATION_ID = x.FirstOrDefault().ORGANIZATION_ID,
                        SUBINVENTORY_CODE = x.FirstOrDefault().SUBINVENTORY_CODE,
                        LOCATOR_ID = x.FirstOrDefault().LOCATOR_ID ?? 0,
                        LOCATOR_SEGMENTS = x.FirstOrDefault().LOCATOR_SEGMENTS,
                        LOCATOR_DESC = x.FirstOrDefault().LOCATOR_DESC,
                        SEGMENT1 = x.FirstOrDefault().SEGMENT1,
                        SEGMENT2 = x.FirstOrDefault().SEGMENT2,
                        SEGMENT3 = x.FirstOrDefault().SEGMENT3,
                        SEGMENT4 = x.FirstOrDefault().SEGMENT4,
                        CONTROL_FLAG = "",
                        LOCATOR_STATUS = x.FirstOrDefault().LOCATOR_STATUS,
                        LOCATOR_STATUS_CODE = x.FirstOrDefault().LOCATOR_STATUS_CODE,
                        LOCATOR_DISABLE_DATE = x.FirstOrDefault().LOCATOR_DISABLE_DATE,
                        LOCATOR_PICKING_ORDER = x.FirstOrDefault().LOCATOR_PICKING_ORDER
                    })
                    .ToList();


                using (var sqlConn = new SqlConnection(MesConnStr))
                {
                    sqlConn.Open();
                    using (var transaction = sqlConn.BeginTransaction())
                    {
                        try
                        {
                            BulkCopier copier = new BulkCopier(sqlConn, transaction);
                            //作業單元、組織、倉庫及儲位 無時間可判定資料新刪修且資料量少，故 全部重抓比對
                            copier.BulkCopy(orgUnitList, "ORG_UNIT_TMP_T", "SP_OrgUnitSync", true);
                            copier.BulkCopy(organizationList, "ORGANIZATION_TMP_T", "SP_OrganizationSync", true);
                            copier.BulkCopy(subList, "SUBINVENTORY_TMP_T", "SP_SubinventorySync", true);
                            copier.BulkCopy(locatorList, "LOCATOR_TMP_T", "SP_LocatorSync", true);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogError($"[{tasker.Name}]-{tasker.Unit}-資料匯入失敗:{ex.Message}");
                        }
                    }
                    sqlConn.Close();
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-結束");
        }
        /// <summary>
        /// 同步 庫存交易類別
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task importTransactionType(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_TRANSACTION_TYPE_V> list = new List<XXCINV_TRANSACTION_TYPE_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        list = (await masterUOW.GetTransactionTypeListAsync()).ToList();

                        if (list.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-無法從{typeof(XXCINV_TRANSACTION_TYPE_V)}VIEW取得資料");
                            return;
                        }
                    }
                }

                var transactionTypeList = list
                    .Select(x => new TRANSACTION_TYPE_TMP_T()
                    {
                        TRANSACTION_TYPE_ID = x.TRANSACTION_TYPE_ID,
                        TRANSACTION_TYPE_NAME = x.TRANSACTION_TYPE_NAME,
                        DESCRIPTION = x.DESCRIPTION,
                        TRANSACTION_ACTION_ID = x.TRANSACTION_ACTION_ID,
                        TRANSACTION_ACTION_NAME = x.TRANSACTION_ACTION_NAME,
                        TRANSACTION_SOURCE_TYPE_ID = x.TRANSACTION_SOURCE_TYPE_ID,
                        TRANSACTION_SOURCE_TYPE_NAME = x.TRANSACTION_SOURCE_TYPE_NAME,
                        CONTROL_FLAG = "",
                        CREATED_BY = x.CREATED_BY,
                        CREATION_DATE = x.CREATION_DATE,
                        LAST_UPDATE_BY = x.LAST_UPDATED_BY,
                        LAST_UPDATE_DATE = x.LAST_UPDATE_DATE
                    })
                    .ToList();

                using (var sqlConn = new SqlConnection(MesConnStr))
                {
                    sqlConn.Open();
                    using (var transaction = sqlConn.BeginTransaction())
                    {
                        try
                        {
                            BulkCopier copier = new BulkCopier(sqlConn, transaction);
                            //資料量少，故 全部重抓比對
                            copier.BulkCopy(transactionTypeList, "TRANSACTION_TYPE_TMP_T", "SP_TransactionTypeSync", true);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogError($"[{tasker.Name}]-{tasker.Unit}-資料匯入失敗:{ex.Message}");
                        }
                    }
                    sqlConn.Close();
                }

            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-結束");

        }
        /// <summary>
        /// 同步 紙別機台
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task importMachinePaperType(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCPO_MACHINE_PAPER_TYPE_V> list = new List<XXCPO_MACHINE_PAPER_TYPE_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        list = (await masterUOW.GetMachinePaperTypeListAsync()).ToList();

                        if (list.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-無法從{typeof(XXCPO_MACHINE_PAPER_TYPE_V)} VIEW取得資料");
                            return;
                        }
                    }
                }

                var machinePaperTypeList = list
                    .Select(x => new MACHINE_PAPER_TYPE_TMP_T()
                    {
                        ORGANIZATION_ID = x.ORGANIZATION_ID,
                        ORGANIZATION_CODE = x.ORGANIZATION_CODE,
                        MACHINE_CODE = x.MACHINE_CODE,
                        MACHINE_MEANING = x.MACHINE_MEANING,
                        DESCRIPTION = x.DESCRIPTION,
                        PAPER_TYPE = x.PAPER_TYPE,
                        MACHINE_NUM = x.MACHINE_NUM,
                        SUPPLIER_NUM = x.SUPPLIER_NUM,
                        SUPPLIER_NAME = x.VENDOR_NAME,
                        CONTROL_FLAG = "",
                        CREATED_BY = x.CREATED_BY,
                        CREATION_DATE = x.CREATION_DATE,
                        LAST_UPDATE_BY = x.LAST_UPDATED_BY,
                        LAST_UPDATE_DATE = x.LAST_UPDATE_DATE
                    })
                    .ToList();

                using (var sqlConn = new SqlConnection(MesConnStr))
                {
                    sqlConn.Open();
                    using (var transaction = sqlConn.BeginTransaction())
                    {
                        try
                        {
                            BulkCopier copier = new BulkCopier(sqlConn, transaction);
                            //資料量少，故 全部重抓比對
                            copier.BulkCopy(machinePaperTypeList, "MACHINE_PAPER_TYPE_TMP_T", "SP_MachinePaperTypeSync", true);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogError($"[{tasker.Name}]-{tasker.Unit}-資料匯入失敗:{ex.Message}");
                        }
                    }
                    sqlConn.Close();
                }

            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-結束");
        }
        /// <summary>
        /// 同步 料號
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task importItem(Tasker tasker, CancellationToken token)
        {
            
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                long sqlItemCount = 0;
                long oraItemAllCount = 0;
                long oraItemCount = 0;
                DateTime? sqlItemLastUpdateDate = null;

                using (var sqlConn = new SqlConnection(MesConnStr))
                {
                    sqlConn.Open();

                    using (var cmd = sqlConn.CreateCommand())
                    {

                        cmd.CommandText = "SELECT MAX(LAST_UPDATE_DATE) FROM ITEMS_TMP_T";
                        var lastUpdateDate = cmd.ExecuteScalar();
                        if(lastUpdateDate != DBNull.Value)
                        {
                            sqlItemLastUpdateDate = Convert.ToDateTime(lastUpdateDate);
                        }
                        //sqlItemLastUpdateDate = (DateTime)cmd.ExecuteScalar();

                        cmd.CommandText = "SELECT COUNT(*) FROM ITEMS_TMP_T";
                        sqlItemCount = Convert.ToInt64(cmd.ExecuteScalar());
                    }

                    using (MasterUOW masterUOW = new MasterUOW(new OracleConnection(ErpConnStr)))
                    {
                        oraItemAllCount = await masterUOW.GetItemCountAsync();
                        oraItemCount = sqlItemLastUpdateDate.HasValue ? (await masterUOW.ItemCountByLastUpdateDateAsync(sqlItemLastUpdateDate.Value)) : 0;
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem- SQL 總筆數:{sqlItemCount} 最後更新日期:{sqlItemLastUpdateDate:yyyy-MM-dd HH:mm:ss.fff}");
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem- ORACLE 總筆數:{oraItemAllCount} 差異筆數:{oraItemCount}");
                        
                        using (SqlTransaction trans = sqlConn.BeginTransaction())
                        {
                            try
                            {
                                 if(oraItemAllCount <= 0 || (oraItemAllCount == sqlItemCount && sqlItemLastUpdateDate.HasValue && oraItemCount <= 0))
                                {
                                    //ERP無資料 或 無差異資料，不動作
                                }
                                else if(sqlItemLastUpdateDate.HasValue && sqlItemCount > 0 && oraItemCount < 1000)
                                {
                                    //更新部分差異
                                    var resultModel = await ImportItemPartially(sqlConn, trans, masterUOW, sqlItemLastUpdateDate.Value, "SP_ItemSync");

                                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-ImportItemPartially:{resultModel}");
                                }
                                else
                                {
                                    //資料全部重抓
                                    var resultModel = await ReimportItem(sqlConn, trans, masterUOW, "ITEMS_TMP_T", "SP_ItemSync");
                                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-ReimportItem:{resultModel}");
                                }
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                sqlConn.Close();
                                throw ex;
                            }
                            finally
                            {
                                sqlConn.Close();
                            }

                        }
                    }
                }

            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-結束");
        }

        /// <summary>
        /// 同步 料號 - 全部重抓
        /// </summary>
        /// <param name="sqlConn"></param>
        /// <param name="trans"></param>
        /// <param name="masterUOW"></param>
        /// <param name="tableName"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        private async Task<ResultModel> ReimportItem(SqlConnection sqlConn, SqlTransaction trans, MasterUOW masterUOW, string tableName, string spName)
        {
            long dividedCount = 5000;

            BulkCopier bCopier = new BulkCopier(sqlConn, trans);

            long allCount = await masterUOW.GetItemCountAsync();
            long count = 0;
            bool notDeleted = true;

            while (count < allCount)
            {
                var list = (await masterUOW.GetItemRangeAsync(count, dividedCount))
                    .Select(x => new ITEMS_TMP_T()
                    {
                        INVENTORY_ITEM_ID = x.INVENTORY_ITEM_ID,
                        ITEM_NUMBER = x.ITEM_NUMBER,
                        ITEM_DESC_ENG = x.ITEM_DESC_ENG,
                        ITEM_DESC_SCH = x.ITEM_DESC_SCH,
                        ITEM_DESC_TCH = x.ITEM_DESC_TCH, 
                        CATEGORY_CODE_INV = x.CATEGORY_CODE_INV,
                        CATEGORY_NAME_INV = x.CATEGORY_NAME_INV,
                        CATEGORY_CODE_COST = x.CATEGORY_CODE_COST,
                        CATEGORY_NAME_COST = x.CATEGORY_NAME_COST,
                        CATEGORY_CODE_CONTROL = x.CATEGORY_CODE_CONTROL,
                        CATEGORY_NAME_CONTROL = x.CATEGORY_NAME_CONTROL,
                        ITEM_TYPE = x.ITEM_TYPE,
                        INVENTORY_ITEM_STATUS_CODE = x.INVENTORY_ITEM_STATUS_CODE,
                        PRIMARY_UOM_CODE = x.PRIMARY_UOM_CODE,
                        SECONDARY_UOM_CODE = x.SECONDARY_UOM_CODE,
                        CATALOG_ELEM_VAL_010 = x.CATALOG_ELEM_VAL_010,
                        CATALOG_ELEM_VAL_020 = x.CATALOG_ELEM_VAL_020,
                        CATALOG_ELEM_VAL_030 = x.CATALOG_ELEM_VAL_030,
                        CATALOG_ELEM_VAL_040 = x.CATALOG_ELEM_VAL_040,
                        CATALOG_ELEM_VAL_050 = x.CATALOG_ELEM_VAL_050,
                        CATALOG_ELEM_VAL_060 = x.CATALOG_ELEM_VAL_060,
                        CATALOG_ELEM_VAL_070 = x.CATALOG_ELEM_VAL_070,
                        CATALOG_ELEM_VAL_080 = x.CATALOG_ELEM_VAL_080,
                        CATALOG_ELEM_VAL_090 = x.CATALOG_ELEM_VAL_090,
                        CATALOG_ELEM_VAL_100 = x.CATALOG_ELEM_VAL_100,
                        CATALOG_ELEM_VAL_110 = x.CATALOG_ELEM_VAL_110,
                        CATALOG_ELEM_VAL_120 = x.CATALOG_ELEM_VAL_120,
                        CATALOG_ELEM_VAL_130 = x.CATALOG_ELEM_VAL_130,
                        CATALOG_ELEM_VAL_140 = x.CATALOG_ELEM_VAL_140,
                        CONTROL_FLAG = "",
                        CREATED_BY = x.CREATED_BY,
                        CREATION_DATE = x.CREATION_DATE,
                        LAST_UPDATE_BY = x.LAST_UPDATED_BY,
                        LAST_UPDATE_DATE = x.LAST_UPDATE_DATE
                    })
                    .ToList();

                count += dividedCount;

                var model = bCopier.BulkCopy(list, tableName, spName, notDeleted, count >= allCount);

                if (!model.Success)
                    return model;

                notDeleted = false;
            }

            return new ResultModel(true, "");
        }

        /// <summary>
        ///  同步 料號 - 更新部分(依最後更新時間來判斷)
        /// </summary>
        /// <param name="sqlConn"></param>
        /// <param name="trans"></param>
        /// <param name="masterUOW"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        private async Task<ResultModel> ImportItemPartially(SqlConnection sqlConn, SqlTransaction trans, MasterUOW masterUOW, DateTime lastUpdateDate, string spName)
        {
            BulkCopier bCopier = new BulkCopier(sqlConn, trans);
            var list = (await masterUOW.GetAllItemByLastUpdateDate(lastUpdateDate))
                    .Select(x => new ITEMS_TMP_T()
                    {
                        INVENTORY_ITEM_ID = x.INVENTORY_ITEM_ID,
                        ITEM_NUMBER = x.ITEM_NUMBER,
                        ITEM_DESC_ENG = x.ITEM_DESC_ENG,
                        ITEM_DESC_SCH = x.ITEM_DESC_SCH,
                        ITEM_DESC_TCH = x.ITEM_DESC_TCH,
                        CATEGORY_CODE_INV = x.CATEGORY_CODE_INV,
                        CATEGORY_NAME_INV = x.CATEGORY_NAME_INV,
                        CATEGORY_CODE_COST = x.CATEGORY_CODE_COST,
                        CATEGORY_NAME_COST = x.CATEGORY_NAME_COST,
                        CATEGORY_CODE_CONTROL = x.CATEGORY_CODE_CONTROL,
                        CATEGORY_NAME_CONTROL = x.CATEGORY_NAME_CONTROL,
                        ITEM_TYPE = x.ITEM_TYPE,
                        INVENTORY_ITEM_STATUS_CODE = x.INVENTORY_ITEM_STATUS_CODE,
                        PRIMARY_UOM_CODE = x.PRIMARY_UOM_CODE,
                        SECONDARY_UOM_CODE = x.SECONDARY_UOM_CODE,
                        CATALOG_ELEM_VAL_010 = x.CATALOG_ELEM_VAL_010,
                        CATALOG_ELEM_VAL_020 = x.CATALOG_ELEM_VAL_020,
                        CATALOG_ELEM_VAL_030 = x.CATALOG_ELEM_VAL_030,
                        CATALOG_ELEM_VAL_040 = x.CATALOG_ELEM_VAL_040,
                        CATALOG_ELEM_VAL_050 = x.CATALOG_ELEM_VAL_050,
                        CATALOG_ELEM_VAL_060 = x.CATALOG_ELEM_VAL_060,
                        CATALOG_ELEM_VAL_070 = x.CATALOG_ELEM_VAL_070,
                        CATALOG_ELEM_VAL_080 = x.CATALOG_ELEM_VAL_080,
                        CATALOG_ELEM_VAL_090 = x.CATALOG_ELEM_VAL_090,
                        CATALOG_ELEM_VAL_100 = x.CATALOG_ELEM_VAL_100,
                        CATALOG_ELEM_VAL_110 = x.CATALOG_ELEM_VAL_110,
                        CATALOG_ELEM_VAL_120 = x.CATALOG_ELEM_VAL_120,
                        CATALOG_ELEM_VAL_130 = x.CATALOG_ELEM_VAL_130,
                        CATALOG_ELEM_VAL_140 = x.CATALOG_ELEM_VAL_140,
                        CONTROL_FLAG = "",
                        CREATED_BY = x.CREATED_BY,
                        CREATION_DATE = x.CREATION_DATE,
                        LAST_UPDATE_BY = x.LAST_UPDATED_BY,
                        LAST_UPDATE_DATE = x.LAST_UPDATE_DATE
                    })
                    .ToList();

            return bCopier.Merge(list, spName, true);

        }

        /// <summary>
        /// 同步 餘切規格
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task importOspRelatedItem(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_OSP_RELATED_ITEM_V> list = new List<XXCINV_OSP_RELATED_ITEM_V>();


                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        list = (await masterUOW.GetOspRelatedItemListAsync()).ToList();

                        if (list.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-無法從{typeof(XXCINV_OSP_RELATED_ITEM_V)} VIEW取得資料");
                            return;
                        }
                    }
                }

                var relatedList = list
                    .Select(x => new RELATED_TMP_T()
                    {
                        INVENTORY_ITEM_ID = x.INVENTORY_ITEM_ID,
                        ITEM_NUMBER = x.ITEM_NUMBER,
                        ITEM_DESCRIPTION = x.ITEM_DESCRIPTION,
                        RELATED_ITEM_ID = x.RELATED_ITEM_ID,
                        RELATED_ITEM_NUMBER = x.RELATED_ITEM_NUMBER,
                        RELATED_ITEM_DESCRIPTION = x.RELATED_ITEM_DESCRIPTION,
                        CONTROL_FLAG = "",
                        CREATED_BY = x.CREATED_BY,
                        CREATION_DATE = x.CREATION_DATE,
                        LAST_UPDATE_BY = x.LAST_UPDATED_BY,
                        LAST_UPDATE_DATE = x.LAST_UPDATE_DATE
                    })
                    .ToList();

                using (var sqlConn = new SqlConnection(MesConnStr))
                {
                    sqlConn.Open();
                    using (var transaction = sqlConn.BeginTransaction())
                    {
                        try
                        {
                            BulkCopier copier = new BulkCopier(sqlConn, transaction);
                            //資料量少，故 全部重抓比對
                            copier.BulkCopy(relatedList, "RELATED_TMP_T", "SP_RelatedSync", true);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-資料匯入失敗:{ex.Message}");
                        }
                    }
                    sqlConn.Close();
                }

            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-結束");
        }
        /// <summary>
        /// 同步 令重包數
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task importYszmpckq(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCOM_YSZMPCKQ_V> list = new List<XXCOM_YSZMPCKQ_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        list = (await masterUOW.GetYszmpckqListAsync()).ToList();

                        if (list.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-無法從{typeof(XXCOM_YSZMPCKQ_V)} VIEW取得資料");
                            return;
                        }
                    }
                }

                var yszmpckqList = list
                    .Select(x => new YSZMPCKQ_TMP_T()
                    {
                        ORGANIZATION_ID = x.ORGANIZATION_ID,
                        ORGANIZATION_CODE = x.ORGANIZATION_CODE,
                        OSP_SUBINVENTORY = x.OSP_SUBINVENTORY,
                        PSTYP = x.PSTYP,
                        BWETUP = x.BWETUP ?? 0m,
                        BWETDN = x.BWETDN ?? 0m,
                        RWTUP = x.RWTUP ?? 0m,
                        RWTDN = x.RWTDN ?? 0m,
                        PCKQ = x.PCKQ ?? 0,
                        PAPER_QTY = x.PAPER_QTY ?? 0,
                        PIECES_QTY = x.PIECES_QTY ?? 0,
                        CONTROL_FLAG = "",
                        CREATED_BY = x.CREATED_BY,
                        CREATION_DATE = x.CREATION_DATE,
                        LAST_UPDATE_BY = x.LAST_UPDATED_BY,
                        LAST_UPDATE_DATE = x.LAST_UPDATE_DATE
                    })
                    .ToList();

                using (var sqlConn = new SqlConnection(MesConnStr))
                {
                    sqlConn.Open();
                    using (var transaction = sqlConn.BeginTransaction())
                    {
                        try
                        {
                            BulkCopier copier = new BulkCopier(sqlConn, transaction);
                            //資料量少，故 全部重抓比對
                            copier.BulkCopy(yszmpckqList, "YSZMPCKQ_TMP_T", "SP_YszmpckqSync", true);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-資料匯入失敗:{ex.Message}");
                        }
                    }
                    sqlConn.Close();
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-結束");
        }

        #endregion 主檔更新 (ORACLE VIEWs TO MES)

        #region SOA同步 (SQL SOA StageTable TO MES)

        /// <summary>
        /// 進櫃SOA排程
        /// </summary>
        internal void AddCtrStTasker()
        {
            AddTasker(new Tasker("進櫃轉檔程序", 1, (tasker, token) => {
                var task = ImportCtrSt(tasker, token);
                var rvTask = task.ContinueWith(subTask => ExportCtrStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(task, rvTask);
            }));
        }

        

        /// <summary>
        /// SOA進櫃入庫 資料下載
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task ImportCtrSt(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var oraConn = new OracleConnection(ErpConnStr);
                using var ctrStUow = new CtrStUOW(sqlConn);
                using var mstUow = new MasterUOW(oraConn);


                var list = await ctrStUow.GetByProcessCodeAsync("XXIFP217");


                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt-無可轉入資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {

                        var st = list?[i];
                        if (st == null)
                        {
                            continue;
                        }
                        //取第一筆資料來確認 新增/取消/修改
                        var ctrSt = await ctrStUow.GetSingleCtrStByAsync(st.PROCESS_CODE, st.SERVER_CODE, st.BATCH_ID, transaction);
                        if (ctrSt == null)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt ({st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-無法取得CONTAINER_ST明細首筆");
                            continue;
                        }


                        ResultModel model = new ResultModel(false, "未知的 ATTRIBUTE1");
                        switch (ctrSt.ATTRIBUTE1.ToUpper())
                        {
                            case "N":
                                model = await ctrStUow.ContainerStReceive(st, transaction);
                                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt ({ctrSt.ATTRIBUTE1}, {st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{model}");
                                if (!model.Success)
                                {
                                    throw new Exception($"code:{model.Code} message:{model.Msg}");
                                }
                                await GenerateCtrPickedOfFlat(ctrSt.PROCESS_CODE, ctrSt.SERVER_CODE, ctrSt.BATCH_ID, mstUow, ctrStUow, transaction);
                                break;
                            case "Y":
                                model = await ctrStUow.ContainerStChange(st, transaction);
                                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt ({ctrSt.ATTRIBUTE1}, {st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{model}");
                                if (!model.Success)
                                {
                                    throw new Exception($"code:{model.Code} message:{model.Msg}");
                                }
                                await GenerateCtrPickedOfFlat(ctrSt.PROCESS_CODE, ctrSt.SERVER_CODE, ctrSt.BATCH_ID, mstUow, ctrStUow, transaction);
                                break;
                            case "C":
                                model = await ctrStUow.ContainerStCancel(st, transaction);
                                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt ({ctrSt.ATTRIBUTE1}, {st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{model}");
                                if (!model.Success)
                                {
                                    throw new Exception($"code:{model.Code} message:{model.Msg}");
                                }
                                break;
                            default:
                                break;
                        }


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt-錯誤-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt-錯誤-{ex.Message}-{ex.StackTrace}");
            }

           LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt-結束");
        }


        internal async Task GenerateCtrPickedOfFlat(string processCode, string serverCode, string batchId, MasterUOW mstUow, CtrStUOW ctrStUow, IDbTransaction transaction = null)
        {
            var detailList = await ctrStUow.GetFlatDetailListBy(processCode, serverCode, batchId, transaction);
            DateTime now = DateTime.Now;
            foreach (var detail in detailList)
            {
                int reamQty = Convert.ToInt32(detail.ROLL_REAM_QTY); //總捲數

                var header = await ctrStUow.GetHeaderById(detail.CTR_HEADER_ID, transaction: transaction);

                if (header == null)
                {
                    throw new Exception($"找不到進櫃檔頭 ID:{detail.CTR_HEADER_ID}");
                }

                var gbModel = await ctrStUow.GenerateBarcodes(header.ORGANIZATION_ID, header.SUBINVENTORY, reamQty, "SYS", "", transaction: transaction);

                if (!gbModel.Success)
                {
                    throw new Exception($"條碼產生出現錯誤 code:{gbModel.Code} message:{gbModel.Msg}");
                }

                var barcodeList = gbModel.Data;

                if (barcodeList.Count != reamQty)
                {
                    throw new Exception($"條碼產生出現錯誤");
                }

                decimal reamWt = detail.ROLL_REAM_WT; //每件令數
                decimal totalSecQty = detail.SECONDARY_QUANTITY ?? 0;

                decimal pryQty = (await mstUow.UomConvertAsync(detail.INVENTORY_ITEM_ID, reamWt, detail.SECONDARY_UOM, detail.PRIMARY_UOM)) ?? 0;
                decimal txnQty = string.Compare(detail.TRANSACTION_UOM, detail.PRIMARY_UOM) == 0 ?
                    pryQty : string.Compare(detail.TRANSACTION_UOM, detail.SECONDARY_UOM) == 0 ?
                    reamWt : ((await mstUow.UomConvertAsync(detail.INVENTORY_ITEM_ID, reamWt, detail.SECONDARY_UOM, detail.TRANSACTION_UOM)) ?? 0);

                for (int j = 0; j < reamQty; j++)
                {
                    if(totalSecQty <= 0)
                    {
                        break;
                    }

                    decimal secQty;
                    if (totalSecQty - reamWt < 0)
                    {
                        //餘重
                        secQty = totalSecQty;

                        pryQty = (await mstUow.UomConvertAsync(detail.INVENTORY_ITEM_ID, secQty, detail.SECONDARY_UOM, detail.PRIMARY_UOM)) ?? 0;
                        txnQty = string.Compare(detail.TRANSACTION_UOM, detail.PRIMARY_UOM) == 0 ?
                            pryQty : string.Compare(detail.TRANSACTION_UOM, detail.SECONDARY_UOM) == 0 ?
                            secQty : ((await mstUow.UomConvertAsync(detail.INVENTORY_ITEM_ID, reamWt, detail.SECONDARY_UOM, detail.TRANSACTION_UOM)) ?? 0);
                        totalSecQty = 0;
                    }
                    else
                    {
                        //滿板
                        secQty = reamWt;
                        totalSecQty -= secQty;
                    }

                    var picked = new CTR_PICKED_T()
                    {
                        CTR_DETAIL_ID = detail.CTR_DETAIL_ID,
                        CTR_HEADER_ID = detail.CTR_HEADER_ID,
                        STOCK_ID = null,
                        LOCATOR_ID = detail.LOCATOR_ID,
                        LOCATOR_CODE = detail.LOCATOR_CODE,
                        BARCODE = barcodeList[j],
                        INVENTORY_ITEM_ID = detail.INVENTORY_ITEM_ID,
                        SHIP_ITEM_NUMBER = detail.SHIP_ITEM_NUMBER,
                        PAPER_TYPE = detail.PAPER_TYPE,
                        BASIC_WEIGHT = detail.BASIC_WEIGHT,
                        REAM_WEIGHT = detail.REAM_WEIGHT,
                        ROLL_REAM_WT = detail.ROLL_REAM_WT,
                        SPECIFICATION = detail.SPECIFICATION,
                        PACKING_TYPE = detail.PACKING_TYPE,
                        SHIP_MT_QTY = detail.SHIP_MT_QTY,
                        TRANSACTION_QUANTITY = txnQty,
                        TRANSACTION_UOM = detail.TRANSACTION_UOM,
                        PRIMARY_QUANTITY = pryQty,
                        PRIMARY_UOM = detail.PRIMARY_UOM,
                        SECONDARY_QUANTITY = secQty,
                        SECONDARY_UOM = detail.SECONDARY_UOM,
                        ITEM_CATEGORY = detail.ITEM_CATEGORY,
                        LOT_NUMBER = "",
                        THEORY_WEIGHT = "",
                        STATUS = "待入庫",
                        REASON_CODE = "",
                        REASON_DESC = "",
                        NOTE = "",
                        CREATED_BY = "SYS",
                        CREATED_USER_NAME = "SYS",
                        CREATION_DATE = now,
                        LAST_UPDATE_BY = null,
                        LAST_UPDATE_DATE = null,
                        LAST_UPDATE_USER_NAME = null
                    };

                    var saveModel = await ctrStUow.SaveCtrPicked(picked, transaction);

                    if(!saveModel.Success)
                    {
                        throw new Exception(saveModel.Msg);
                    }
                }

            }
        }
        /// <summary>
        /// SOA進櫃入庫 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task ExportCtrStRv(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrStRv-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                using var ctrStUow = new CtrStUOW(new SqlConnection(MesConnStr));


            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrStRv-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ImportCtrStRv-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrStRv-結束");
        }

        #endregion SOA同步 (SQL SOA StageTable TO MES)

        /// <summary>
        /// LOG - 一般
        /// </summary>
        /// <param name="message"></param>
        private void LogInfo(string message)
        {
            logger.Info(message);
            Console.WriteLine(message);
        }

        /// <summary>
        /// LOG - 錯誤
        /// </summary>
        /// <param name="message"></param>
        private void LogError(string message)
        {
            logger.Error(message);
            Console.WriteLine(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (mainForm != null && !mainForm.IsDisposed)
                        mainForm.Dispose();
                    if (uomConverterForm != null && !uomConverterForm.IsDisposed)
                        uomConverterForm.Dispose();
                    if (masterViewForm != null && !masterViewForm.IsDisposed)
                        masterViewForm.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
