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
    public class DlvStService
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        internal string ErpConnStr { set; get; }

        internal string MesConnStr { set; get; }


        public DlvStService()
        {

        }

        public DlvStService(string mesConStr, string erpConStr)
        {
            MesConnStr = mesConStr;
            ErpConnStr = erpConStr;
        }


        /// <summary>
        /// SOA出貨 資料下載
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task ImportDlvSt(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportDlvSt-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var oraConn = new OracleConnection(ErpConnStr);
                using var dlvStUow = new DlvStUOW(sqlConn);
                using var mstUow = new MasterUOW(oraConn);


                var list = await dlvStUow.GetByProcessCodeAsync("XXIFP220");


                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportDlvSt-無可轉入資料");
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


                        ResultModel model = new ResultModel(false, "未知的錯誤!");

                        model = await dlvStUow.DeliveryStReceive(st, transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportDlvSt ({st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{model}");
                        if (!model.Success)
                        {
                            throw new Exception($"code:{model.Code} message:{model.Msg}");
                        }


                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ImportDlvSt-錯誤-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportDlvSt-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ImportDlvSt-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportDlvSt-結束");
        }


        /// <summary>
        /// SOA出貨 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task ExportDlvStRv(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportDlvStRv-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                using var sqlConn = new SqlConnection(MesConnStr);
                using var dlvStUow = new DlvStUOW(sqlConn);
                var list = await dlvStUow.GetTripList();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportDlvStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = await dlvStUow.DeliveryStUpload(list[i], transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportDlvStRv (TRIP_ID:{list[i]})-{model}");

                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportDlvStRv-錯誤-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }

            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportDlvStRv-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportDlvStRv-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportDlvStRv-結束");
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
