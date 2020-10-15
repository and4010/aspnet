using CHPOUTSRCMES.TASK.Models.Entity;
using CHPOUTSRCMES.TASK.Models.Repository.Interface;
using CHPOUTSRCMES.TASK.Models.Repository.MsSql;
using CHPOUTSRCMES.TASK.Models.Views;
using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Transactions;

namespace CHPOUTSRCMES.TASK.Models.UnitOfWork
{
    public class OspStUOW : BaseUOW
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        //private CtrStRepository ctrStRepository = null;
        //public CtrStRepository CtrStRepository => ctrStRepository ??
        //    (ctrStRepository = new CtrStRepository(Context, $"{SchemaName}XXIF_CHP_P217_CONTAINER_ST"));

        private OspBatchStRepository ospBatchStRepository = null;
        public OspBatchStRepository OspBatchStRepository => ospBatchStRepository ??
            (ospBatchStRepository = new OspBatchStRepository(Context, $"{SchemaName}XXIF_CHP_P219_OSP_BATCH_ST"));

        private OspHeaderRepository ospHeaderRepository = null;
        public OspHeaderRepository OspHeaderRepository => ospHeaderRepository ??
            (ospHeaderRepository = new OspHeaderRepository(Context, $"{SchemaName}OSP_HEADER_T"));

        private OspSoaS1Repository ospSoaS1Repository = null;
        public OspSoaS1Repository OspSoaS1Repository => ospSoaS1Repository ??
            (ospSoaS1Repository = new OspSoaS1Repository(Context, $"{SchemaName}OSP_SOA_S1_T"));

        private OspSoaS2Repository ospSoaS2Repository = null;
        public OspSoaS2Repository OspSoaS2Repository => ospSoaS2Repository ??
            (ospSoaS2Repository = new OspSoaS2Repository(Context, $"{SchemaName}OSP_SOA_S2_T"));

        private OspSoaS3Repository ospSoaS3Repository = null;
        public OspSoaS3Repository OspSoaS3Repository => ospSoaS3Repository ??
            (ospSoaS3Repository = new OspSoaS3Repository(Context, $"{SchemaName}OSP_SOA_S3_T"));


        public OspStUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
        }

        #region OSP_SOA_S1_T

        public async Task<List<long>> GetOspBatchStage1UploadList(IDbTransaction transaction = null)
        {
            return (await OspSoaS1Repository.GetUploadList(transaction));
        }

        public async Task<List<OSP_SOA_S1_T>> GetOspBatchStage1UploadedList(IDbTransaction transaction = null)
        {
            return (await OspSoaS1Repository.GetUploadedList(transaction));
        }

        public async Task<ResultModel> UpdateStatusCode(OSP_SOA_S1_T data, IDbTransaction transaction = null)
        {
            return ( await OspSoaS1Repository.UpdateStatusCode(data, transaction) );

        }

        public async Task<ResultModel> OspBatchStStage1Upload(long ospHeaderId, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@ospHeaderId", value: ospHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@processCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@serverCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@batchId", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P210_OspStStage1Upload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }

            return resultModel;
        }

        #endregion

        #region OSP_SOA_S2_T

        public async Task<List<long>> GetOspBatchStage2UploadList(IDbTransaction transaction = null)
        {
            return (await OspSoaS2Repository.GetUploadList(transaction));
        }

        public async Task<List<OSP_SOA_S2_T>> GetOspBatchStage2UploadedList(IDbTransaction transaction = null)
        {
            return (await OspSoaS2Repository.GetUploadedList(transaction));
        }

        public async Task<ResultModel> UpdateStage2StatusCode(OSP_SOA_S2_T data, IDbTransaction transaction = null)
        {
            return (await OspSoaS2Repository.UpdateStatusCode(data, transaction));

        }

        public async Task<ResultModel> OspBatchStStage2Upload(long ospHeaderId, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@ospHeaderId", value: ospHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@processCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@serverCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@batchId", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P211_OspStStage2Upload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }

            return resultModel;
        }


        #endregion


        #region OSP_SOA_S3_T

        public async Task<List<long>> GetOspBatchStage3UploadList(IDbTransaction transaction = null)
        {
            return (await OspSoaS3Repository.GetUploadList(transaction));
        }

        public async Task<List<OSP_SOA_S3_T>> GetOspBatchStage3UploadedList(IDbTransaction transaction = null)
        {
            return (await OspSoaS3Repository.GetUploadedList(transaction));
        }

        public async Task<ResultModel> UpdateStage3StatusCode(OSP_SOA_S3_T data, IDbTransaction transaction = null)
        {
            return (await OspSoaS3Repository.UpdateStatusCode(data, transaction));

        }

        public async Task<ResultModel> OspBatchStStage3Upload(long ospHeaderId, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@ospHeaderId", value: ospHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P213_OspStStage3Upload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }

            return resultModel;
        }


        #endregion

        //public async Task<List<long>> GetOspBatchStage3List(IDbTransaction transaction = null)
        //{
        //    return (await OspHeaderRepository.GetStage3List(transaction));
        //}

        //public async Task<List<long>> GetOspBatchExportedStage3List(IDbTransaction transaction = null)
        //{
        //    return (await OspHeaderRepository.GetExportedStage3List(transaction));
        //}

        public ResultModel OspBatchStReceive(XXIF_CHP_CONTROL_ST controlStage, IDbTransaction transaction = null)
        {

            var task = OspBatchStRepository.GetListBy(controlStage.PROCESS_CODE, controlStage.SERVER_CODE, controlStage.BATCH_ID, transaction);
            task.Wait();
            var list = task.Result;

            if (list.Count == 0)
                return new ResultModel(false, "");

            using var trans = connection.BeginTransaction();
            bool rollback = false;
            for (int i = 0; i < list.Count; i++)
            {
                ResultModel result = null;

                if(list[i].LINE_TYPE.CompareTo("P") == 0)
                {
                    continue;
                }


                switch (list[i].BATCH_STATUS)
                {
                    case 2:
                        result = OspBatchStCreateNow(list[i], trans);
                        break;
                    case -1:
                        result = OspBatchStCancel(list[i], trans);
                        break;
                    default:
                        result = OspBatchStChange(list[i], trans);
                        break;
                }

                if (result.Code != ResultModel.CODE_SUCCESS)
                {
                    list[i].STATUS_CODE = "E";
                    list[i].ERROR_MSG = result.Msg;
                    rollback = true;
                }
                
            }

            if (rollback)
            {
                trans.Rollback();
            }
            else
            {
                trans.Commit();
            }


            for (int i = 0; i < list.Count; i++)
            {
                OspBatchStRepository.UpdateStatus(list[i]).Wait();
                System.Threading.Thread.Sleep(100);
            }
            
            return OspBatchStSummarize(controlStage, transaction);

        }


        public ResultModel OspBatchStCreateNow(XXIF_CHP_P219_OSP_BATCH_ST st, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: st.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: st.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: st.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@peBatchId", value: st.PE_BATCH_ID, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P219_OspStCreateNew", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }

        public ResultModel OspBatchStCheck(string processCode, string serverCode, string batchId, string userId = "SYS", IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();
            
            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P219_CheckOspBatchSt", param: p,transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if(!resultModel.Success)
                {
                    return resultModel;
                }
            }
            catch(Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }

        public ResultModel OspBatchStChange(XXIF_CHP_P219_OSP_BATCH_ST st, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: st.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: st.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: st.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@peBatchId", value: st.PE_BATCH_ID, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P219_OspStChange", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }

        public ResultModel OspBatchStCancel(XXIF_CHP_P219_OSP_BATCH_ST st, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: st.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: st.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: st.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@peBatchId", value: st.PE_BATCH_ID, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P219_OspStCancel", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }

        public ResultModel OspBatchStSummarize(XXIF_CHP_CONTROL_ST st, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: st.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: st.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: st.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P219_OspStSummarize", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }


        public async Task<XXIF_CHP_CONTROL_ST> GetControlSt(string processCode, string serverCode, string batchId)
        {
            return await ControlStageRepository.GetBy(processCode, serverCode, batchId);
        }

    }
}
