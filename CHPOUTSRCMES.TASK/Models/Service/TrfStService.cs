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
    public class TrfStService
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        internal string ErpConnStr { set; get; }

        internal string MesConnStr { set; get; }


        public TrfStService()
        {

        }

        public TrfStService(string mesConStr, string erpConStr)
        {
            MesConnStr = mesConStr;
            ErpConnStr = erpConStr;
        }

        /// <summary>
        /// SOA庫存異動 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal void ExportTrfStRv(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportTrfStRv-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var oraConn = new OracleConnection(ErpConnStr);
                using var sqlConn = new SqlConnection(MesConnStr);
                using var trfStUow = new TrfStUOW(sqlConn);
                using var masterUow = new MasterUOW(oraConn);

                var list = trfStUow.GetHeaderListForUpload();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportTrfStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = trfStUow.TransferStUpload(list[i], masterUow, transaction: transaction);

                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportTrfStRv (TRF_HEADER_ID:{list[i]})-{model}");
                        
                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();
                        
                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportTrfStRv-錯誤-(TRF_HEADER_ID:{list[i]})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }

                    Thread.Sleep(100); //避免出現同一個BATCH_ID
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportTrfStRv-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportTrfStRv-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportTrfStRv-結束");
        }

        /// <summary>
        /// SOA貨故原因 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal void ExportRsnStRv(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportReasonStRv-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var oraConn = new OracleConnection(ErpConnStr);
                using var sqlConn = new SqlConnection(MesConnStr);
                using var trfStUow = new TrfStUOW(sqlConn);
                using var masterUow = new MasterUOW(oraConn);
                var list = trfStUow.GetReasonHeaderListForUpload();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportReasonStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = trfStUow.TrfRsnStUpload(list[i], masterUow,transaction: transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportReasonStRv (TRANSFER_REASON_HEADER_ID:{list[i]})-{model}");

                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportReasonStRv-錯誤-(TRANSFER_REASON_HEADER_ID:{list[i]})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }

                    Thread.Sleep(100); //避免出現同一個BATCH_ID
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportReasonStRv-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportReasonStRv-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportReasonStRv-結束");
        }

        /// <summary>
        /// SOA盤點 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal void ExportInvStRv(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportInvStRv-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var oraConn = new OracleConnection(ErpConnStr);
                using var sqlConn = new SqlConnection(MesConnStr);
                using var trfStUow = new TrfStUOW(sqlConn);
                using var masterUow = new MasterUOW(oraConn);
                var list = trfStUow.GetInvHeaderListForUpload();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportInvStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = trfStUow.TrfInvStUpload(list[i], masterUow, transaction: transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportInvStRv (TRANSFER_INVENTORY_HEADER_ID:{list[i]})-{model}");

                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportInvStRv-錯誤-(TRANSFER_INVENTORY_HEADER_ID:{list[i]})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }

                    Thread.Sleep(100); //避免出現同一個BATCH_ID
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportInvStRv-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportInvStRv-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportInvStRv-結束");
        }

        /// <summary>
        /// SOA雜項異動 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal void ExportMiscStRv(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportMiscStRv-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var oraConn = new OracleConnection(ErpConnStr);
                using var sqlConn = new SqlConnection(MesConnStr);
                using var trfStUow = new TrfStUOW(sqlConn);
                using var masterUow = new MasterUOW(oraConn);
                var list = trfStUow.GetMiscHeaderListForUpload();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportMiscStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = trfStUow.TrfMiscStUpload(list[i], masterUow, transaction: transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportMiscStRv (TRANSFER_MISCELLANEOUS_HEADER_ID:{list[i]})-{model}");

                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportMiscStRv-錯誤-(TRANSFER_MISCELLANEOUS_HEADER_ID:{list[i]})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }

                    Thread.Sleep(100); //避免出現同一個BATCH_ID
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportMiscStRv-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportMiscStRv-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportMiscStRv-結束");
        }

        /// <summary>
        /// SOA存貨報癈 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal void ExportObsStRv(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportObsStRv-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var oraConn = new OracleConnection(ErpConnStr);
                using var sqlConn = new SqlConnection(MesConnStr);
                using var trfStUow = new TrfStUOW(sqlConn);
                using var masterUow = new MasterUOW(oraConn);
                var list = trfStUow.GetObsHeaderListForUpload();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportObsStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = trfStUow.TrfObsStUpload(list[i], masterUow, transaction: transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportObsStRv (TRANSFER_OBSOLETE_HEADER_ID:{list[i]})-{model}");

                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportObsStRv-錯誤-(TRANSFER_OBSOLETE_HEADER_ID:{list[i]})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }

                    Thread.Sleep(100); //避免出現同一個BATCH_ID
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportObsStRv-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportObsStRv-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportObsStRv-結束");
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
