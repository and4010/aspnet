using CHPOUTSRCMES.TASK.Models.Entity;
using CHPOUTSRCMES.TASK.Models.Entity.Temp;
using CHPOUTSRCMES.TASK.Models.UnitOfWork;
using CHPOUTSRCMES.TASK.Models.Views;
using CHPOUTSRCMES.TASK.Tasks;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Service
{
    public class MasterService
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        internal string ErpConnStr { set; get; }

        internal string MesConnStr { set; get; }


        public MasterService()
        {

        }

        public MasterService(string mesConStr, string erpConStr)
        {
            MesConnStr = mesConStr;
            ErpConnStr = erpConStr;
        }


        /// <summary>
        /// 同步 作業單元、組織、倉庫、儲位 資料表
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task importSubinventory(Tasker tasker, CancellationToken token)
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
                    .Select(x => new ORGANIZATION_TMP_T()
                    {
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


                using var sqlConn = new SqlConnection(MesConnStr);
                sqlConn.Open();
                using (SqlTransaction transaction = sqlConn.BeginTransaction())
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
        internal async Task importTransactionType(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_TRANSACTION_TYPE_V> list = new List<XXCINV_TRANSACTION_TYPE_V>();

                using (OracleConnection conn = new OracleConnection(ErpConnStr))
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

                using var sqlConn = new SqlConnection(MesConnStr);
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
        internal async Task importMachinePaperType(Tasker tasker, CancellationToken token)
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
        internal async Task importItem(Tasker tasker, CancellationToken token)
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
                        if (lastUpdateDate != DBNull.Value)
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
                                if (oraItemAllCount <= 0 || (oraItemAllCount == sqlItemCount && sqlItemLastUpdateDate.HasValue && oraItemCount <= 0))
                                {
                                    //ERP無資料 或 無差異資料，不動作
                                }
                                else if (sqlItemLastUpdateDate.HasValue && sqlItemCount > 0 && oraItemCount < 1000)
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
        internal async Task<ResultModel> ReimportItem(SqlConnection sqlConn, SqlTransaction trans, MasterUOW masterUOW, string tableName, string spName)
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
        internal async Task<ResultModel> ImportItemPartially(SqlConnection sqlConn, SqlTransaction trans, MasterUOW masterUOW, DateTime lastUpdateDate, string spName)
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
        internal async Task importOspRelatedItem(Tasker tasker, CancellationToken token)
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
        internal async Task importYszmpckq(Tasker tasker, CancellationToken token)
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
    }
}
