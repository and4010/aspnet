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
    public class DlvStUOW : BaseUOW
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        //private CtrStRepository ctrStRepository = null;
        //public CtrStRepository CtrStRepository => ctrStRepository ??
        //    (ctrStRepository = new CtrStRepository(Context, $"{SchemaName}XXIF_CHP_P217_CONTAINER_ST"));

        private DlvHeaderRepository dlvHeaderRepository = null;
        public DlvHeaderRepository DlvHeaderRepository => dlvHeaderRepository ??
            (dlvHeaderRepository = new DlvHeaderRepository(Context, $"{SchemaName}DLV_HEADER_T"));

        private DlvDetailRepository dlvDetailRepository = null;
        public DlvDetailRepository DlvDetailRepository => dlvDetailRepository ??
            (dlvDetailRepository = new DlvDetailRepository(Context, $"{SchemaName}DLV_DETAIL_T"));


        public DlvStUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
        }

        public async Task<DLV_HEADER_T> GetHeaderById(long headerId, IDbTransaction transaction = null)
        {
            return (await DlvHeaderRepository.GetAsync(headerId, transaction));
        }

        public async Task<ResultModel> DeliveryStReceive(XXIF_CHP_CONTROL_ST controlStage, IDbTransaction transaction = null)
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

                Context.Execute(sql: "SP_P220_DlvStCreateNew", param: p,transaction: transaction, commandType: CommandType.StoredProcedure);
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

    }
}
