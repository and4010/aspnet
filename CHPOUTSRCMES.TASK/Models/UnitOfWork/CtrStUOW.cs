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

        private CtrHeaderRepository ctrHeaderRepository = null;
        public CtrHeaderRepository CtrHeaderRepository => ctrHeaderRepository ??
            (ctrHeaderRepository = new CtrHeaderRepository(Context, $"{SchemaName}CTR_HEADER_T"));

        private CtrDetailRepository ctrDetailRepository = null;
        public CtrDetailRepository CtrDetailRepository => ctrDetailRepository ??
            (ctrDetailRepository = new CtrDetailRepository(Context, $"{SchemaName}CTR_DETAIL_T"));

        private CtrPickedRepository ctrPickedRepository = null;
        public CtrPickedRepository CtrPickedRepository => ctrPickedRepository ??
            (ctrPickedRepository = new CtrPickedRepository(Context, $"{SchemaName}CTR_PICKED_T"));

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

        public async Task<List<CTR_DETAIL_T>> GetFlatDetailListBy(string processCode, string serverCode, string batchId, IDbTransaction transaction = null)
        {
            return (await CtrDetailRepository.GetFlatListBy(processCode, serverCode, batchId, transaction))?.ToList();
        }

        public async Task<CTR_HEADER_T> GetHeaderById(long headerId, IDbTransaction transaction = null)
        {
            return (await CtrHeaderRepository.GetAsync(headerId, transaction));
        }

        public async Task<ResultDataModel<CTR_PICKED_T>> SaveCtrPicked(CTR_PICKED_T ctrPicked, IDbTransaction transaction = null)
        {
            ResultDataModel<CTR_PICKED_T> model;
            try
            {
                var id = await CtrPickedRepository.InsertAsync(ctrPicked, transaction: transaction);

                if (id.HasValue)
                {
                    ctrPicked.CTR_PICKED_ID = id.Value;
                    model = new ResultDataModel<CTR_PICKED_T>(true, "", ctrPicked);
                }
                else
                {
                    model = new ResultDataModel<CTR_PICKED_T>(false, "CTR_PICKED_T 寫入失敗", null);
                }
                
            }
            catch(Exception ex)
            {
                model = new ResultDataModel<CTR_PICKED_T>(false, $"CTR_PICKED_T 寫入失敗{ex.Message} {ex.StackTrace}", null);
            }
            return model;
        }


        public async Task<ResultModel> ContainerStReceive(XXIF_CHP_CONTROL_ST controlStage, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();
            
            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: controlStage.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: controlStage.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: controlStage.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P217_CtrStCreateNew", param: p,transaction: transaction, commandType: CommandType.StoredProcedure);
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

        public async Task<ResultModel> ContainerStChange(XXIF_CHP_CONTROL_ST contolStage, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();
            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: contolStage.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: contolStage.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: contolStage.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P217_CtrStChange", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }

        public async Task<ResultModel> ContainerStCancel(XXIF_CHP_CONTROL_ST contolStage, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();
            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@processCode", value: contolStage.PROCESS_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@serverCode", value: contolStage.SERVER_CODE, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@batchId", value: contolStage.BATCH_ID, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P217_CtrStCancel", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;
        }


        /// <summary>
        /// 產生條碼清單 (請用交易TRANSACTION)
        /// </summary>
        /// <param name="organiztionId">組織ID</param>
        /// <param name="subinventoryCode">倉庫</param>
        /// <param name="prefix">前置碼</param>
        /// <param name="requestQty">數量</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>ResultDataModel 條碼清單</returns>
        public async Task<ResultDataModel<List<string>>> GenerateBarcodes(long organiztionId, string subinventoryCode, int requestQty, string userId, string prefix = "", IDbTransaction transaction = null)
        {
            ResultDataModel<List<string>> result = null;
            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@organizationId", value: organiztionId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@subinventory", value: subinventoryCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@prefix", value: prefix, dbType: DbType.String, direction: ParameterDirection.Input, size: 1);
                p.Add(name: "@@requestQty", value: requestQty, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);


                var list = (await Context.QueryAsync<string>(sql: "SP_GenerateBarcodes", param: p, transaction: transaction, commandType: CommandType.StoredProcedure)).ToList();
                result = new ResultDataModel<List<string>>(p.Get<int>("@code"), p.Get<string>("@message"), list);
            }
            catch (Exception ex)
            {
                result = new ResultDataModel<List<string>>(-1, ex.Message, null);
                logger.Error(ex, "產生條碼出現例外!!");
            }
            return result;
        }

    }
}
