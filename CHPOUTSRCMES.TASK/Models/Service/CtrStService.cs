using CHPOUTSRCMES.TASK.Models.Entity;
using CHPOUTSRCMES.TASK.Models.UnitOfWork;
using CHPOUTSRCMES.TASK.Tasks;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Service
{
    public class CtrStService
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        internal string ErpConnStr { set; get; }

        internal string MesConnStr { set; get; }


        public CtrStService()
        {

        }

        public CtrStService(string mesConStr, string erpConStr)
        {
            MesConnStr = mesConStr;
            ErpConnStr = erpConStr;
        }


        /// <summary>
        /// SOA進櫃入庫 資料下載
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal void ImportCtrSt(Tasker tasker, CancellationToken token)
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


                var list = ctrStUow.GetByProcessCode("XXIFP217");
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt-無可轉入資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();

                    var st = list?[i];
                    if (st == null)
                    {
                        continue;
                    }

                    try
                    {

                        //取第一筆資料來確認 新增/取消/修改
                        var ctrSt = ctrStUow.GetSingleCtrStBy(st.PROCESS_CODE, st.SERVER_CODE, st.BATCH_ID, transaction);
                        if (ctrSt == null)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt ({st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-無法取得CONTAINER_ST明細首筆");
                            continue;
                        }


                        ResultModel model = new ResultModel(false, "未知的 ATTRIBUTE1");
                        switch (ctrSt.ATTRIBUTE1.ToUpper())
                        {
                            case "N":
                                model = ctrStUow.ContainerStReceive(st, transaction);
                                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt ({ctrSt.ATTRIBUTE1}, {st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{model}");
                                if (!model.Success)
                                {
                                    throw new Exception($"code:{model.Code} message:{model.Msg}");
                                }
                                GenerateCtrPickedOfFlat(ctrSt.PROCESS_CODE, ctrSt.SERVER_CODE, ctrSt.BATCH_ID, mstUow, ctrStUow, transaction);
                                break;
                            case "Y":
                                model = ctrStUow.ContainerStChange(st, transaction);
                                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt ({ctrSt.ATTRIBUTE1}, {st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{model}");
                                if (!model.Success)
                                {
                                    throw new Exception($"code:{model.Code} message:{model.Msg}");
                                }
                                GenerateCtrPickedOfFlat(ctrSt.PROCESS_CODE, ctrSt.SERVER_CODE, ctrSt.BATCH_ID, mstUow, ctrStUow, transaction);
                                break;
                            case "C":
                                model = ctrStUow.ContainerStCancel(st, transaction);
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
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ImportCtrSt-錯誤-({st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{ex.Message}-{ex.StackTrace}");
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


        internal void GenerateCtrPickedOfFlat(string processCode, string serverCode, string batchId, MasterUOW mstUow, CtrStUOW ctrStUow, IDbTransaction transaction = null)
        {
            var task = ctrStUow.GetFlatDetailListBy(processCode, serverCode, batchId, transaction);
            task.Wait();
            var detailList = task.Result;
            
            DateTime now = DateTime.Now;
            foreach (var detail in detailList)
            {
                int reamQty = Convert.ToInt32(detail.ROLL_REAM_QTY); //總捲數
                var taskHeader = ctrStUow.GetHeaderById(detail.CTR_HEADER_ID, transaction: transaction);
                taskHeader.Wait();
                var header = taskHeader.Result;

                if (header == null)
                {
                    throw new Exception($"找不到進櫃檔頭 ID:{detail.CTR_HEADER_ID}");
                }
                var taskBarcode = ctrStUow.GenerateBarcodes(header.ORGANIZATION_ID, header.SUBINVENTORY, reamQty, "SYS", "", transaction: transaction);
                taskBarcode.Wait();
                var gbModel = taskBarcode.Result;

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

                decimal pryQty = (mstUow.UomConvert(detail.INVENTORY_ITEM_ID, reamWt, detail.SECONDARY_UOM, detail.PRIMARY_UOM)) ?? 0;
                decimal txnQty = string.Compare(detail.TRANSACTION_UOM, detail.PRIMARY_UOM) == 0 ?
                    pryQty : string.Compare(detail.TRANSACTION_UOM, detail.SECONDARY_UOM) == 0 ?
                    reamWt : ((mstUow.UomConvert(detail.INVENTORY_ITEM_ID, reamWt, detail.SECONDARY_UOM, detail.TRANSACTION_UOM)) ?? 0);

                for (int j = 0; j < reamQty; j++)
                {
                    if (totalSecQty <= 0)
                    {
                        break;
                    }

                    decimal secQty;
                    if (totalSecQty - reamWt < 0)
                    {
                        //餘重
                        secQty = totalSecQty;

                        pryQty = (mstUow.UomConvert(detail.INVENTORY_ITEM_ID, secQty, detail.SECONDARY_UOM, detail.PRIMARY_UOM)) ?? 0;
                        txnQty = string.Compare(detail.TRANSACTION_UOM, detail.PRIMARY_UOM) == 0 ?
                            pryQty : string.Compare(detail.TRANSACTION_UOM, detail.SECONDARY_UOM) == 0 ?
                            secQty : ((mstUow.UomConvert(detail.INVENTORY_ITEM_ID, reamWt, detail.SECONDARY_UOM, detail.TRANSACTION_UOM)) ?? 0);
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
                    var taskSaveCtrPicked = ctrStUow.SaveCtrPicked(picked, transaction);
                    taskSaveCtrPicked.Wait();
                    var saveModel = taskSaveCtrPicked.Result;

                    if (!saveModel.Success)
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
        internal void ExportCtrStRv(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportCtrStRv-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var ctrStUow = new CtrStUOW(sqlConn);
                var task = ctrStUow.GetHeaderListForUpload();
                task.Wait();
                var list = task.Result;

                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportCtrStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = ctrStUow.ContainerStUpload(list[i].CTR_HEADER_ID, transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportCtrStRv (CTR_HEADER_ID:{list[i].CTR_HEADER_ID})-{model}");
                        
                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();
                        
                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportCtrStRv-錯誤-(CTR_HEADER_ID:{list[i].CTR_HEADER_ID})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                    Thread.Sleep(100);
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportCtrStRv-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportCtrStRv-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportCtrStRv-結束");
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
