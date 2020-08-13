﻿using CHPOUTSRCMES.TASK.Forms;
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
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK
{
    public class MainController :IDisposable
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

        internal MainForm MainForm => mainForm ?? (mainForm = new MainForm() { Controller = this});
        
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
        internal async void StopTimer()
        {
            if (!cancelTokenSource.IsCancellationRequested)
            {
                cancelTokenSource.Cancel();
            }

            try
            {
                await Task.WhenAll(taskList.Values.ToArray());
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
            taskers.Add(tasker);
        }

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

        internal void AddMasterTasker()
        {
            AddTasker(new Tasker("主檔轉檔程序", 1, ImportMaster));
        }

        internal async void ImportMaster(Tasker tasker, CancellationToken token)
        {
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

            await Task.WhenAll(taskSubinventory, taskTransactionType, taskItem, taskOspRelatedItem, taskMachinePaperType, tasYszmpckq);
        }

        private async Task importSubinventory(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_SUBINVENTORY_V> subinventoryList = new List<XXCINV_SUBINVENTORY_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        subinventoryList = (await masterUOW.GetSubinventoryListAsync()).ToList();

                        if (subinventoryList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-無法從 {typeof(XXCINV_SUBINVENTORY_V)}VIEW取得資料");
                            return;
                        }
                    }
                }

                var orgUnitList = subinventoryList
                    .GroupBy(x => x.ORG_ID)
                    .Select(x => new ORG_UNIT_TMP_T()
                    {
                        ORG_ID = x.FirstOrDefault().ORG_ID,
                        ORG_NAME = x.FirstOrDefault().ORG_NAME,
                        CONTROL_FLAG = ""
                    })
                    .ToList();

                //取得組織
                var organizationList = subinventoryList
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
                var subList = subinventoryList
                    .GroupBy(x => new { x.ORGANIZATION_ID, x.SUBINVENTORY_CODE } )
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
                var locatorList = subinventoryList
                    .Where(x=> x.LOCATOR_ID != null && x.LOCATOR_ID > 0)
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
                            copier.BulkCopy(orgUnitList,        "ORG_UNIT_TMP_T",       true);
                            copier.BulkCopy(organizationList,   "ORGANIZATION_TMP_T",   true);
                            copier.BulkCopy(subList,            "SUBINVENTORY_TMP_T",   true);
                            copier.BulkCopy(locatorList,        "LOCATOR_TMP_T",        true);

                            transaction.Commit();
                        }
                        catch(Exception ex)
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
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-結束");
        }

        private async Task importItem(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXIFV050_ITEMS_FTY_V> itemList = new List<XXIFV050_ITEMS_FTY_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        itemList = (await masterUOW.GetItemListAsync()).ToList();

                        if (itemList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-無法從{typeof(XXIFV050_ITEMS_FTY_V)} VIEW取得資料");
                            return;
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

        private async Task importOspRelatedItem(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_OSP_RELATED_ITEM_V> itemList = new List<XXCINV_OSP_RELATED_ITEM_V>();


                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        itemList = (await masterUOW.GetOspRelatedItemListAsync()).ToList();

                        if (itemList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-無法從{typeof(XXCINV_OSP_RELATED_ITEM_V)} VIEW取得資料");
                            return;
                        }
                    }
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

        private async Task importMachinePaperType(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCPO_MACHINE_PAPER_TYPE_V> machinePaperTypeList = new List<XXCPO_MACHINE_PAPER_TYPE_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        machinePaperTypeList = (await masterUOW.GetMachinePaperTypeListAsync()).ToList();

                        if (machinePaperTypeList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-無法從{typeof(XXCPO_MACHINE_PAPER_TYPE_V)} VIEW取得資料");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-結束");
        }

        private async Task importYszmpckq(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCOM_YSZMPCKQ_V> yszmpckqList = new List<XXCOM_YSZMPCKQ_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        yszmpckqList = (await masterUOW.GetYszmpckqListAsync()).ToList();

                        if (yszmpckqList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-無法從{typeof(XXCOM_YSZMPCKQ_V)} VIEW取得資料");
                            return;
                        }
                    }
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

        private async Task importTransactionType(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_TRANSACTION_TYPE_V> transactionTypeList = new List<XXCINV_TRANSACTION_TYPE_V>();

                using (var conn = new OracleConnection(ErpConnStr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        transactionTypeList = (await masterUOW.GetTransactionTypeListAsync()).ToList();

                        if (transactionTypeList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-無法從{typeof(XXCINV_TRANSACTION_TYPE_V)}VIEW取得資料");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-結束");

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
