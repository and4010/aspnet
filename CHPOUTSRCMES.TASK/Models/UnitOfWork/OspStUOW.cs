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

        private InMmtIngredientStRepository inMmtIngredientStRepository = null;
        public InMmtIngredientStRepository InMmtIngredientStRepository => inMmtIngredientStRepository ??
            (inMmtIngredientStRepository = new InMmtIngredientStRepository(Context, $"{SchemaName}XXIF_CHP_P210_IN_MMT_INGR_ST"));

        private InMmtProductStRepository inMmtProductStRepository = null;
        public InMmtProductStRepository InMmtProductStRepository => inMmtProductStRepository ??
            (inMmtProductStRepository = new InMmtProductStRepository(Context, $"{SchemaName}XXIF_CHP_P211_IN_MMT_PROD_ST"));

        public OspStUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
        }

        #region OSP_SOA_S1_T

        public List<long> GetOspBatchStage1UploadList(IDbTransaction transaction = null)
        {
            return OspSoaS1Repository.GetUploadList(transaction);
        }

        public List<OSP_SOA_S1_T> GetOspBatchStage1UploadedList(IDbTransaction transaction = null)
        {
            return OspSoaS1Repository.GetUploadedList(transaction);
        }

        public ResultModel UpdateStatusCode(OSP_SOA_S1_T data, IDbTransaction transaction = null)
        {
            return OspSoaS1Repository.UpdateStatusCode(data, transaction);

        }

        public ResultModel OspBatchStStage1Upload(long ospHeaderId, MasterUOW masterUOW, string userId = "SYS", IDbTransaction transaction = null)
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
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P210_OspStStage1Upload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }

                string processCode = p.Get<string>("@processCode");
                string serverCode = p.Get<string>("@serverCode");
                string batchId = p.Get<string>("@batchId");

                //換算主單位及交易單位
                resultModel = updateInMmtIngrStList(processCode, serverCode, batchId, masterUOW, transaction);
                if (!resultModel.Success)
                {
                    return resultModel;
                }

                //回寫 CONTROL_ST
                var paramSoa = new DynamicParameters();
                paramSoa.Add(name: "@ospHeaderId", value: ospHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                paramSoa.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                paramSoa.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                paramSoa.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);
                Context.Execute(sql: "SP_P210_InMmtIngrStSummarize", param: paramSoa, transaction: transaction, commandType: CommandType.StoredProcedure);

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


        public ResultModel updateInMmtIngrStList(string processCode, string serverCode, string batchId, MasterUOW masterUOW, IDbTransaction transaction = null)
        {
            var list = InMmtIngredientStRepository.GetListBy(processCode, serverCode, batchId, transaction);
            if (list == null || list.Count == 0)
            {
                return new ResultModel(false, "無資料可上傳");
            }

            for (int i = 0; i < list.Count; i++)
            {
                var m = list[i];
                var itemId = InMmtIngredientStRepository.GetInventoryItemId(m.ITEM_NO, transaction);
                if (m.SECONDARY_TRANSACTION_QUANTITY.HasValue && m.SECONDARY_TRANSACTION_QUANTITY.Value > 0 && !string.IsNullOrEmpty(m.SECONDARY_UOM_CODE))
                {
                    var primaryQuantity = masterUOW.UomConvert(itemId, m.SECONDARY_TRANSACTION_QUANTITY.Value, m.SECONDARY_UOM_CODE, m.TRANSACTION_UOM);
                    if (!primaryQuantity.HasValue)
                    {
                        throw new Exception($"單位換算失敗!! ITEM ID :{itemId} AMOUNT:{m.SECONDARY_TRANSACTION_QUANTITY.Value} UOM:{m.SECONDARY_UOM_CODE} TO:{m.TRANSACTION_UOM}");
                    }

                    m.TRANSACTION_QUANTITY = primaryQuantity.Value;
                }
                else
                {
                    var primaryQuantity = masterUOW.UomConvert(itemId, m.TRANSACTION_QUANTITY, "KG", m.TRANSACTION_UOM);
                    if (!primaryQuantity.HasValue)
                    {
                        throw new Exception($"單位換算失敗!! ITEM ID :{itemId} AMOUNT:{m.TRANSACTION_QUANTITY} UOM:KG TO:{m.TRANSACTION_UOM}");
                    }

                    m.TRANSACTION_QUANTITY = primaryQuantity.Value;
                }
                InMmtIngredientStRepository.Update(m, transaction);

            }

            return new ResultModel(true, "");
        }


        #endregion

        #region OSP_SOA_S2_T

        public List<long> GetOspBatchStage2UploadList(IDbTransaction transaction = null)
        {
            return OspSoaS2Repository.GetUploadList(transaction);
        }

        public List<OSP_SOA_S2_T> GetOspBatchStage2UploadedList(IDbTransaction transaction = null)
        {
            return OspSoaS2Repository.GetUploadedList(transaction);
        }

        public ResultModel UpdateStage2StatusCode(OSP_SOA_S2_T data, IDbTransaction transaction = null)
        {
            return OspSoaS2Repository.UpdateStatusCode(data, transaction);

        }

        public ResultModel OspBatchStStage2Upload(long ospHeaderId, MasterUOW masterUOW, string userId = "SYS", IDbTransaction transaction = null)
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
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P211_OspStStage2Upload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }

                string processCode = p.Get<string>("@processCode");
                string serverCode = p.Get<string>("@serverCode");
                string batchId = p.Get<string>("@batchId");

                //換算主單位及交易單位
                resultModel = updateInMmtProdStList(processCode, serverCode, batchId, masterUOW, transaction);
                if (!resultModel.Success)
                {
                    return resultModel;
                }

                //回寫 CONTROL_ST
                var paramSoa = new DynamicParameters();
                paramSoa.Add(name: "@ospHeaderId", value: ospHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                paramSoa.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                paramSoa.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                paramSoa.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);
                Context.Execute(sql: "SP_P211_InMmtProdStSummarize", param: paramSoa, transaction: transaction, commandType: CommandType.StoredProcedure);

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

        public ResultModel updateInMmtProdStList(string processCode, string serverCode, string batchId, MasterUOW masterUOW, IDbTransaction transaction = null)
        {
            var list = InMmtProductStRepository.GetListBy(processCode, serverCode, batchId, transaction);
            if (list == null || list.Count == 0)
            {
                return new ResultModel(false, "無資料可上傳");
            }

            for (int i = 0; i < list.Count; i++)
            {
                var m = list[i];
                var itemId = InMmtProductStRepository.GetInventoryItemId(m.ITEM_NO, transaction);
                if (m.SECONDARY_TRANSACTION_QUANTITY.HasValue && m.SECONDARY_TRANSACTION_QUANTITY.Value > 0 && !string.IsNullOrEmpty(m.SECONDARY_UOM_CODE))
                {
                    var primaryQuantity = masterUOW.UomConvert(itemId, m.SECONDARY_TRANSACTION_QUANTITY.Value, m.SECONDARY_UOM_CODE, m.TRANSACTION_UOM);
                    if (!primaryQuantity.HasValue)
                    {
                        throw new Exception($"單位換算失敗!! ITEM ID :{itemId} AMOUNT:{m.SECONDARY_TRANSACTION_QUANTITY.Value} UOM:{m.SECONDARY_UOM_CODE} TO:{m.TRANSACTION_UOM}");
                    }

                    m.TRANSACTION_QUANTITY = primaryQuantity.Value;
                }
                else
                {
                    var primaryQuantity = masterUOW.UomConvert(itemId, m.TRANSACTION_QUANTITY, "KG", m.TRANSACTION_UOM);
                    if (!primaryQuantity.HasValue)
                    {
                        throw new Exception($"單位換算失敗!! ITEM ID :{itemId} AMOUNT:{m.TRANSACTION_QUANTITY} UOM:KG TO:{m.TRANSACTION_UOM}");
                    }

                    m.TRANSACTION_QUANTITY = primaryQuantity.Value;
                }
                InMmtProductStRepository.Update(m, transaction);

            }

            return new ResultModel(true, "");
        }

        #endregion


        #region OSP_SOA_S3_T

        public List<long> GetOspBatchStage3UploadList(IDbTransaction transaction = null)
        {
            return OspSoaS3Repository.GetUploadList(transaction);
        }

        public List<OSP_SOA_S3_T> GetOspBatchStage3UploadedList(IDbTransaction transaction = null)
        {
            return OspSoaS3Repository.GetUploadedList(transaction);
        }

        public ResultModel UpdateStage3StatusCode(OSP_SOA_S3_T data, IDbTransaction transaction = null)
        {
            return OspSoaS3Repository.UpdateStatusCode(data, transaction);

        }

        public ResultModel OspBatchStStage3Upload(long ospHeaderId, string userId = "SYS",IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@ospHeaderId", value: ospHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

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

        public ResultModel OspBatchStReceive(XXIF_CHP_CONTROL_ST controlStage, IDbTransaction transaction = null)
        {

            var list = OspBatchStRepository.GetListBy(controlStage.PROCESS_CODE, controlStage.SERVER_CODE, controlStage.BATCH_ID, transaction);

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
                OspBatchStRepository.UpdateStatus(list[i]);
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


        public XXIF_CHP_CONTROL_ST GetControlSt(string processCode, string serverCode, string batchId)
        {
            return ControlStageRepository.GetBy(processCode, serverCode, batchId);
        }

    }
}
