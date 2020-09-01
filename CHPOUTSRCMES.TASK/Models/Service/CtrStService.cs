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
                    if (totalSecQty <= 0)
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
