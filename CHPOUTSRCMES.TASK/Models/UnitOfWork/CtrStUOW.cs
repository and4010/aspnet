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

namespace CHPOUTSRCMES.TASK.Models.UnitOfWork
{
    public class CtrStUOW : UnitOfWork
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

#if DEBUG
        private string schemaName = "dbo";
#else
        private string schemaName = "dbo";
#endif

        public string SchemaName
        {
            set { schemaName = value; }
            get { return schemaName + (schemaName.Length > 0 ? "." : ""); }
        }

        private ControlStageRepository controlStageRepository = null;
        public ControlStageRepository ControlStageRepository => controlStageRepository ??
            (controlStageRepository = new ControlStageRepository(Context, $"{SchemaName}XXIF_CHP_CONTROL_ST"));

        private CtrStRepository ctrStRepository = null;
        public CtrStRepository CtrStRepository => ctrStRepository ??
            (ctrStRepository = new CtrStRepository(Context, $"{SchemaName}XXIF_CHP_P217_CONTAINER_ST"));



        public CtrStUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
        }

        public async Task<XXIF_CHP_P217_CONTAINER_ST> GetSingleCtrStByAsync(string processCode, string serverCode, string batchId, IDbTransaction transaction = null)
        {
            return await CtrStRepository.GetSingleBy(processCode, serverCode, batchId, transaction: transaction);
        }

        public async Task<List<XXIF_CHP_CONTROL_ST>> GetByProcessCodeAsync(string processCode, string soaProcessCode = "S", string pullingFlag = "Out-S", IDbTransaction transaction = null)
        {
            return (await ControlStageRepository.GetControlStageListBy(processCode, soaProcessCode, pullingFlag, transaction:transaction))?.ToList();
        }

        public async Task<ResultModel> ContainerStReceive(XXIF_CHP_CONTROL_ST contolStage)
        {
            var resultModel = new ResultModel();
            using var trans = Context.BeginTransaction();
            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: contolStage.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: contolStage.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: contolStage.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P217_CtrStCreateNew", param: p,transaction: trans, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));
                trans.Commit();
            }
            catch(Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }

        public async Task<ResultModel> ContainerStChange(XXIF_CHP_CONTROL_ST contolStage)
        {
            var resultModel = new ResultModel();
            using var trans = Context.BeginTransaction();
            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: contolStage.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: contolStage.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: contolStage.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P217_CtrStChange", param: p, transaction: trans, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));
                trans.Commit();
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }

        public async Task<ResultModel> ContainerStCancel(XXIF_CHP_CONTROL_ST contolStage)
        {
            var resultModel = new ResultModel();
            using var trans = Context.BeginTransaction();
            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: contolStage.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: contolStage.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: contolStage.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P217_CtrStCancel", param: p, transaction: trans, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));
                trans.Commit();
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }

    }
}
