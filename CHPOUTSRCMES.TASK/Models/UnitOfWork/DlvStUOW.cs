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

        private ShipConfirmStRepository shipConfirmStRepository = null;
        public ShipConfirmStRepository ShipConfirmStRepository => shipConfirmStRepository ??
            (shipConfirmStRepository = new ShipConfirmStRepository(Context, $"{SchemaName}XXIF_CHP_P221_SHIP_CONFIRM_ST"));


        public DlvStUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
        }

        public async Task<DLV_HEADER_T> GetHeaderById(long headerId, IDbTransaction transaction = null)
        {
            return (await DlvHeaderRepository.GetAsync(headerId, transaction));
        }

        public async Task<List<long>> GetTripList(IDbTransaction transaction = null)
        {
            return (await DlvHeaderRepository.GetUploadList(transaction));
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

        public async Task<ResultModel> DeliveryStUpload(long TripId, MasterUOW masterUOW, string userId = "SYS", IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@tripId", value: TripId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@processCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@serverCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@batchId", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P221_DlvStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }

                //換算主單位及交易單位
                string processCode = p.Get<string>("@processCode");
                string serverCode = p.Get<string>("@serverCode");
                string batchId = p.Get<string>("@batchId");

                var list = await ShipConfirmStRepository.GetListBy(processCode, serverCode, batchId, transaction);
                if (list == null || list.Count == 0)
                {
                    return new ResultModel(false, "DeliveryStUpload 無資料可上傳");
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var m = list[i];
                    if (m.SECONDARY_QUANTITY.HasValue && m.SECONDARY_QUANTITY.Value > 0 && !string.IsNullOrEmpty(m.SECONDARY_UOM))
                    {
                        var primaryQuantity = await masterUOW.UomConvertAsync(m.INVENTORY_ITEM_ID, m.SECONDARY_QUANTITY.Value, m.SECONDARY_UOM, m.PRIMARY_UOM);
                        if (!primaryQuantity.HasValue)
                        {
                            throw new Exception($"單位換算失敗!! ITEM ID :{m.INVENTORY_ITEM_ID} AMOUNT:{m.SECONDARY_QUANTITY} UOM:{m.SECONDARY_UOM} TO:{m.PRIMARY_UOM}");
                        }

                        m.PRIMARY_QUANTITY = primaryQuantity.Value;

                        var txnQuantity = await masterUOW.UomConvertAsync(m.INVENTORY_ITEM_ID, m.SECONDARY_QUANTITY.Value, m.SECONDARY_UOM, m.TRANSACTION_UOM);
                        if (!txnQuantity.HasValue)
                        {
                            throw new Exception($"單位換算失敗!! ITEM ID :{m.INVENTORY_ITEM_ID} AMOUNT:{m.SECONDARY_QUANTITY} UOM:{m.SECONDARY_UOM} TO:{m.TRANSACTION_UOM}");
                        }

                        m.TRANSACTION_QUANTITY = txnQuantity.Value;
                    }
                    else
                    {
                        var txnQuantity = await masterUOW.UomConvertAsync(m.INVENTORY_ITEM_ID, m.PRIMARY_QUANTITY, m.PRIMARY_UOM, m.TRANSACTION_UOM);
                        if (!txnQuantity.HasValue)
                        {
                            throw new Exception($"單位換算失敗!! ITEM ID :{m.INVENTORY_ITEM_ID} AMOUNT:{m.PRIMARY_QUANTITY} UOM:{m.PRIMARY_UOM} TO:{m.TRANSACTION_UOM}");
                        }

                        m.TRANSACTION_QUANTITY = txnQuantity.Value;
                        m.ROLL_QUANTITY = Math.Abs(m.PRIMARY_QUANTITY).ToString("0.#####");
                    }
                    await ShipConfirmStRepository.Update(m, transaction);

                }

                //回寫 CONTROL_ST
                var paramSoa = new DynamicParameters();
                paramSoa.Add(name: "@tripId", value: TripId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                paramSoa.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                paramSoa.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                paramSoa.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);
                Context.Execute(sql: "SP_P221_ShipConfirmSummarize", param: paramSoa, transaction: transaction, commandType: CommandType.StoredProcedure);

                resultModel = new ResultModel(paramSoa.Get<int>("@code"), paramSoa.Get<string>("@message"));

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

    }
}
