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
    public class BaseUOW : UnitOfWork
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

#if BIOTECH
        private string schemaName = "dbo";
#else
        private string schemaName = "dbo";
#endif

        public IDbConnection connection; 

        public string SchemaName
        {
            set { schemaName = value; }
            get { return schemaName + (schemaName.Length > 0 ? "." : ""); }
        }

        private ControlStageRepository controlStageRepository = null;
        public ControlStageRepository ControlStageRepository => controlStageRepository ??
            (controlStageRepository = new ControlStageRepository(Context, $"{SchemaName}XXIF_CHP_CONTROL_ST"));


        public BaseUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
            this.connection = conn;
        }


        public List<XXIF_CHP_CONTROL_ST> GetByProcessCodeAsync(string processCode, string soaProcessCode = "S", string pullingFlag = "Out-S", IDbTransaction transaction = null)
        {
            return ControlStageRepository.GetControlStageListBy(processCode, soaProcessCode, pullingFlag, transaction:transaction)?.ToList();
        }

        public List<XXIF_CHP_CONTROL_ST> GetByProcessCode(string processCode, string soaProcessCode = "S", string pullingFlag = "Out-S", IDbTransaction transaction = null)
        {
            return ControlStageRepository.GetControlStageListBy(processCode, soaProcessCode, pullingFlag, transaction: transaction).ToList();
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
        public ResultDataModel<List<string>> GenerateBarcodes(long organiztionId, string subinventoryCode, int requestQty, string userId, string prefix = "", IDbTransaction transaction = null)
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


                var list = Context.Query<string>(sql: "SP_GenerateBarcodes", param: p, transaction: transaction, commandType: CommandType.StoredProcedure).ToList();
                result = new ResultDataModel<List<string>>(p.Get<int>("@code"), p.Get<string>("@message"), list);
            }
            catch (Exception ex)
            {
                result = new ResultDataModel<List<string>>(-1, ex.Message, null);
                logger.Error(ex, "產生條碼出現例外!!");
            }
            return result;
        }

        public ResultModel UpdateStatus(XXIF_CHP_CONTROL_ST st, IDbTransaction transaction = null)
        {
            return ControlStageRepository.UpdateStatus(st, transaction);
        }

    }
}
