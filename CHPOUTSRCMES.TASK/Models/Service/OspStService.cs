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
    public class OspStService
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        internal string ErpConnStr { set; get; }

        internal string MesConnStr { set; get; }


        public OspStService()
        {

        }

        public OspStService(string mesConStr, string erpConStr)
        {
            MesConnStr = mesConStr;
            ErpConnStr = erpConStr;
        }


        /// <summary>
        /// SOA出貨 P219 資料下載
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task ImportOspSt(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportOspSt-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var oraConn = new OracleConnection(ErpConnStr);
                using var ospStUow = new OspStUOW(sqlConn);
                using var mstUow = new MasterUOW(oraConn);


                var list = await ospStUow.GetByProcessCodeAsync("XXIFP219");


                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportOspSt-無可轉入資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    var st = list?[i];
                    if (st == null)
                    {
                        continue;
                    }

                    try
                    {
                        var model = ospStUow.OspBatchStCheck(st.PROCESS_CODE, st.SERVER_CODE, st.BATCH_ID);

                        if (model.Code != ResultModel.CODE_SUCCESS)
                        {
                            throw new Exception($"code:{model.Code} message:{model.Msg}");
                        }

                        Thread.Sleep(300);
                        model = ospStUow.OspBatchStReceive(st);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportOspSt ({st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{model}");
                        if (model.Code != ResultModel.CODE_SUCCESS)
                        {
                            throw new Exception($"code:{model.Code} message:{model.Msg}");
                        }

                    }
                    catch (Exception ex)
                    {
                        st.STATUS_CODE = "E";
                        st.ERROR_MSG = ex.Message;
                        ospStUow.UpdateStatus(st).Wait();
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ImportOspSt-錯誤-({st.PROCESS_CODE}, {st.SERVER_CODE}, {st.BATCH_ID})-{ex.Message}-{ex.StackTrace}");
                    }
                    Thread.Sleep(300);
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportOspSt-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ImportOspSt-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ImportOspSt-結束");
        }


        /// <summary>
        /// SOA出貨 P210 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task ExportOspStRvStage1(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage1-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var ospStUow = new OspStUOW(sqlConn);

                var list = await ospStUow.GetOspBatchStage1UploadList();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = await ospStUow.OspBatchStStage1Upload(list[i], transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage1 (OSP_HEADER_ID:{list[i]})-{model}");

                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage1-錯誤-(OSP_HEADER_ID:{list[i]})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }


            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage1-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage1-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage1-結束");
        }

        /// <summary>
        /// SOA出貨 P210 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task UpdateStatusOspStRvStage1(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage1-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var ospStUow = new OspStUOW(sqlConn);

                var list = await ospStUow.GetOspBatchStage1UploadedList();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage1-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    var data = list[i];
                    var controlSt = await ospStUow.ControlStageRepository.GetBy(data.PROCESS_CODE, data.SERVER_CODE, data.BATCH_ID, pullingFlag: "In-S");

                    if(controlSt == null 
                        || string.IsNullOrEmpty(data.PROCESS_CODE) 
                        || string.IsNullOrEmpty(data.SERVER_CODE) 
                        || string.IsNullOrEmpty(data.BATCH_ID))
                    {
                        continue;
                    }

                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        data.STATUS_CODE = controlSt.STATUS_CODE;
                        data.LAST_UPDATE_BY = "SYS";
                        data.LAST_UPDATE_DATE = DateTime.Now;

                        var model = await ospStUow.OspSoaS1Repository.UpdateStatusCode(data, transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage1 (OSP_HEADER_ID:{data.OSP_HEADER_ID}, PROCESS_CODE:{data.PROCESS_CODE}, SERVER_CODE:{data.SERVER_CODE}, BATCH_ID:{data.BATCH_ID})-{model}");
                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage1-錯誤-(OSP_HEADER_ID:{data.OSP_HEADER_ID}, PROCESS_CODE:{data.PROCESS_CODE}, SERVER_CODE:{data.SERVER_CODE}, BATCH_ID:{data.BATCH_ID})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }


            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage1-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage1-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage1-結束");
        }


        /// <summary>UpdateStatusOspStRvStage2
        /// SOA出貨 P211 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task ExportOspStRvStage2(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage2-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var ospStUow = new OspStUOW(sqlConn);

                var list = await ospStUow.GetOspBatchStage2UploadList();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = await ospStUow.OspBatchStStage2Upload(list[i], transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage2 (OSP_HEADER_ID:{list[i]})-{model}");

                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage2-錯誤-(OSP_HEADER_ID:{list[i]})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }


            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage2-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage2-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage2-結束");
        }

        /// <summary>
        /// SOA出貨 P211 資料上傳 狀態回寫
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task UpdateStatusOspStRvStage2(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage2-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var ospStUow = new OspStUOW(sqlConn);

                var list = await ospStUow.GetOspBatchStage2UploadedList();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage2-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    var data = list[i];
                    var controlSt = await ospStUow.ControlStageRepository.GetBy(data.PROCESS_CODE, data.SERVER_CODE, data.BATCH_ID, pullingFlag: "In-S");

                    if (controlSt == null
                        || string.IsNullOrEmpty(data.PROCESS_CODE)
                        || string.IsNullOrEmpty(data.SERVER_CODE)
                        || string.IsNullOrEmpty(data.BATCH_ID))
                    {
                        continue;
                    }

                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        data.STATUS_CODE = controlSt.STATUS_CODE;
                        data.LAST_UPDATE_BY = "SYS";
                        data.LAST_UPDATE_DATE = DateTime.Now;

                        var model = await ospStUow.OspSoaS2Repository.UpdateStatusCode(data, transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage2 (OSP_HEADER_ID:{data.OSP_HEADER_ID}, PROCESS_CODE:{data.PROCESS_CODE}, SERVER_CODE:{data.SERVER_CODE}, BATCH_ID:{data.BATCH_ID})-{model}");
                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage2-錯誤-(OSP_HEADER_ID:{data.OSP_HEADER_ID})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }


            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage2-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage2-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage2-結束");
        }


        /// <summary>
        /// SOA出貨 P213 資料上傳
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task ExportOspStRvStage3(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage3-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var ospStUow = new OspStUOW(sqlConn);

                var list = await ospStUow.GetOspBatchStage3UploadList();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRv-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        var model = await ospStUow.OspBatchStStage3Upload(list[i], transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage3 (OSP_HEADER_ID:{list[i]})-{model}");

                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage3-錯誤-(OSP_HEADER_ID:{list[i]})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }


            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage3-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage3-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-ExportOspStRvStage3-結束");
        }

        /// <summary>
        /// SOA出貨 P213 資料上傳 狀態回寫
        /// </summary>
        /// <param name="tasker"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal async Task UpdateStatusOspStRvStage3(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage3-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                using var sqlConn = new SqlConnection(MesConnStr);
                using var ospStUow = new OspStUOW(sqlConn);

                var list = await ospStUow.GetOspBatchStage3UploadedList();
                if (list == null || list.Count() == 0)
                {
                    LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage3-無可轉出資料");
                    return;
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    var data = list[i];
                    var controlSt = await ospStUow.ControlStageRepository.GetBy(data.PROCESS_CODE, data.SERVER_CODE, data.BATCH_ID, pullingFlag: "In-S");

                    if (controlSt == null
                        || string.IsNullOrEmpty(data.PROCESS_CODE)
                        || string.IsNullOrEmpty(data.SERVER_CODE)
                        || string.IsNullOrEmpty(data.BATCH_ID))
                    {
                        continue;
                    }

                    using var transaction = sqlConn.BeginTransaction();
                    try
                    {
                        data.STATUS_CODE = controlSt.STATUS_CODE;
                        data.LAST_UPDATE_BY = "SYS";
                        data.LAST_UPDATE_DATE = DateTime.Now;

                        var model = await ospStUow.OspSoaS3Repository.UpdateStatusCode(data, transaction);
                        LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage3 (OSP_HEADER_ID:{data.OSP_HEADER_ID}, PROCESS_CODE:{data.PROCESS_CODE}, SERVER_CODE:{data.SERVER_CODE}, BATCH_ID:{data.BATCH_ID})-{model}");
                        if (!model.Success)
                        {
                            throw new Exception(model.Msg);
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        LogError($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage3-錯誤-(OSP_HEADER_ID:{data.OSP_HEADER_ID})-{ex.Message}-{ex.StackTrace}");
                        transaction.Rollback();
                    }
                }


            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage3-使用者取消");
            }
            catch (Exception ex)
            {
                LogError($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage3-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-UpdateStatusOspStRvStage3-結束");
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
