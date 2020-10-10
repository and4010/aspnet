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
    public class TrfStUOW : BaseUOW
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private TrfHeaderRepository trfHeaderRepository = null;
        public TrfHeaderRepository TrfHeaderRepository => trfHeaderRepository ??
            (trfHeaderRepository = new TrfHeaderRepository(Context, $"{SchemaName}TRF_HEADER_T"));

        private TrfRsnHeaderRepository trfRsnHeaderRepository = null;
        public TrfRsnHeaderRepository TrfRsnHeaderRepository => trfRsnHeaderRepository ??
            (trfRsnHeaderRepository = new TrfRsnHeaderRepository(Context, $"{SchemaName}TRF_REASON_HEADER_T"));

        private TrfInvHeaderRepository trfInvHeaderRepository = null;
        public TrfInvHeaderRepository TrfInvHeaderRepository => trfInvHeaderRepository ??
            (trfInvHeaderRepository = new TrfInvHeaderRepository(Context, $"{SchemaName}TRF_INVENTORY_HEADER_T"));

        private TrfMiscHeaderRepository trfMiscHeaderRepository = null;
        public TrfMiscHeaderRepository TrfMiscHeaderRepository => trfMiscHeaderRepository ??
            (trfMiscHeaderRepository = new TrfMiscHeaderRepository(Context, $"{SchemaName}TRF_MISCELLANEOUS_HEADER_T"));

        private TrfObsHeaderRepository trfObsHeaderRepository = null;
        public TrfObsHeaderRepository TrfObsHeaderRepository => trfObsHeaderRepository ??
            (trfObsHeaderRepository = new TrfObsHeaderRepository(Context, $"{SchemaName}TRF_OBSOLETE_HEADER_T"));

        private SubTransferStRepository subTransferStRepository = null;
        public SubTransferStRepository SubTransferStRepository => subTransferStRepository ??
            (subTransferStRepository = new SubTransferStRepository(Context, $"{SchemaName}XXIF_CHP_P222_SUB_TRANSFER_ST"));


        public TrfStUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
        }

        public async Task<TRF_HEADER_T> GetHeaderById(long headerId, IDbTransaction transaction = null)
        {
            return (await TrfHeaderRepository.GetAsync(headerId, transaction));
        }

        public async Task<List<long>> GetHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await TrfHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TransferStUpload(long trfHeaderId, MasterUOW masterUOW, string userId = "SYS", IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                //寫入 庫存轉移 XXIF_CHP_P222_SUB_TRANSFER_ST
                var p = new DynamicParameters();
                p.Add(name: "@trfHeaderId", value: trfHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                p.Add(name: "@processCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@serverCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@batchId", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }

                //換算主單位及交易單位
                string processCode = p.Get<string>("@processCode");
                string serverCode = p.Get<string>("@serverCode");
                string batchId = p.Get<string>("@batchId");
                var list = await SubTransferStRepository.GetListBy(processCode, serverCode, batchId, transaction);
                if(list == null || list.Count == 0)
                {
                    return new ResultModel(false, "TransferStUpload 無資料可上傳");
                }

                for(int i= 0; i < list.Count; i ++)
                {
                    var m = list[i];
                    if (m.SECONDARY_QUANTITY > 0 && !string.IsNullOrEmpty(m.SECONDARY_UOM_CODE))
                    {
                        var primaryQuantity = await masterUOW.UomConvertAsync(m.INVENTORY_ITEM_ID, m.SECONDARY_QUANTITY, m.SECONDARY_UOM_CODE, m.PRIMARY_UOM);
                        if (!primaryQuantity.HasValue)
                        {
                            throw new Exception($"單位換算失敗!! ITEM ID :{m.INVENTORY_ITEM_ID} AMOUNT:{m.PRIMARY_UOM} UOM:{m.SECONDARY_UOM_CODE} TO:{m.PRIMARY_UOM}");
                        }

                        m.PRIMARY_QUANTITY = primaryQuantity.Value;
                        m.TRANSACTION_QUANTITY = m.SECONDARY_QUANTITY;
                        m.TRANSACTION_UOM = m.SECONDARY_UOM_CODE;
                    }
                    else
                    {
                        m.TRANSACTION_QUANTITY = m.PRIMARY_QUANTITY;
                        m.TRANSACTION_UOM = m.PRIMARY_UOM;
                        m.ROLL_QUANTITY = Math.Abs(m.PRIMARY_QUANTITY).ToString("0.#####");
                    }
                    SubTransferStRepository.Update(m, transaction);
                }

                //回寫 CONTROL_ST
                var paramSoa = new DynamicParameters();
                paramSoa.Add(name: "@trfRsnHeaderId", value: trfHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                paramSoa.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                paramSoa.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                paramSoa.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);
                Context.Execute(sql: "SP_P222_TrfStSummarize", param: paramSoa, transaction: transaction, commandType: CommandType.StoredProcedure);
                
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


        public async Task<List<long>> GetReasonHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await TrfRsnHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TrfRsnStUpload(long trfRsnHeaderId, MasterUOW masterUOW, string userId = "SYS", IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@@trfRsnHeaderId", value: trfRsnHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@processCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@serverCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@batchId", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfRsnStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }

                //換算主單位及交易單位
                string processCode = p.Get<string>("@processCode");
                string serverCode = p.Get<string>("@serverCode");
                string batchId = p.Get<string>("@batchId");
                var list = await SubTransferStRepository.GetListBy(processCode, serverCode, batchId, transaction);
                if (list == null || list.Count == 0)
                {
                    return new ResultModel(false, "TrfRsnStUpload 無資料可上傳");
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var m = list[i];
                    if (m.SECONDARY_QUANTITY > 0 && !string.IsNullOrEmpty(m.SECONDARY_UOM_CODE))
                    {
                        var primaryQuantity = await masterUOW.UomConvertAsync(m.INVENTORY_ITEM_ID, m.SECONDARY_QUANTITY, m.SECONDARY_UOM_CODE, m.PRIMARY_UOM);
                        if (!primaryQuantity.HasValue)
                        {
                            throw new Exception($"單位換算失敗!! ITEM ID :{m.INVENTORY_ITEM_ID} AMOUNT:{m.PRIMARY_UOM} UOM:{m.SECONDARY_UOM_CODE} TO:{m.PRIMARY_UOM}");
                        }

                        m.PRIMARY_QUANTITY = primaryQuantity.Value;
                        m.TRANSACTION_QUANTITY = m.SECONDARY_QUANTITY;
                        m.TRANSACTION_UOM = m.SECONDARY_UOM_CODE;
                    }
                    else
                    {
                        m.TRANSACTION_QUANTITY = m.PRIMARY_QUANTITY;
                        m.TRANSACTION_UOM = m.PRIMARY_UOM;
                        m.ROLL_QUANTITY = Math.Abs(m.PRIMARY_QUANTITY).ToString("0.#####");
                    }
                    await SubTransferStRepository.Update(m, transaction);
                    
                }

                //回寫 CONTROL_ST
                var paramSoa = new DynamicParameters();
                paramSoa.Add(name: "@trfRsnHeaderId", value: trfRsnHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                paramSoa.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                paramSoa.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                paramSoa.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);
                Context.Execute(sql: "SP_P222_TrfRsnStSummarize", param: paramSoa, transaction: transaction, commandType: CommandType.StoredProcedure);

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


        public async Task<List<long>> GetInvHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await TrfRsnHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TrfInvStUpload(long trfInvHeaderId, MasterUOW masterUOW, string userId = "SYS", IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@trfInvHeaderId", value: trfInvHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@processCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@serverCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@batchId", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfInvStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }

                //換算主單位及交易單位
                string processCode = p.Get<string>("@processCode");
                string serverCode = p.Get<string>("@serverCode");
                string batchId = p.Get<string>("@batchId");

                var list = await SubTransferStRepository.GetListBy(processCode, serverCode, batchId, transaction);
                if (list == null || list.Count == 0)
                {
                    return new ResultModel(false, "TrfInvStUpload 無資料可上傳");
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var m = list[i];
                    if (m.SECONDARY_QUANTITY > 0 && !string.IsNullOrEmpty(m.SECONDARY_UOM_CODE))
                    {
                        var primaryQuantity = await masterUOW.UomConvertAsync(m.INVENTORY_ITEM_ID, m.SECONDARY_QUANTITY, m.SECONDARY_UOM_CODE, m.PRIMARY_UOM);
                        if (!primaryQuantity.HasValue)
                        {
                            throw new Exception($"單位換算失敗!! ITEM ID :{m.INVENTORY_ITEM_ID} AMOUNT:{m.PRIMARY_UOM} UOM:{m.SECONDARY_UOM_CODE} TO:{m.PRIMARY_UOM}");
                        }

                        m.PRIMARY_QUANTITY = primaryQuantity.Value;
                        m.TRANSACTION_QUANTITY = m.SECONDARY_QUANTITY;
                        m.TRANSACTION_UOM = m.SECONDARY_UOM_CODE;
                    }
                    else
                    {
                        m.TRANSACTION_QUANTITY = m.PRIMARY_QUANTITY;
                        m.TRANSACTION_UOM = m.PRIMARY_UOM;
                        m.ROLL_QUANTITY = Math.Abs(m.PRIMARY_QUANTITY).ToString("0.#####");
                    }
                    await SubTransferStRepository.Update(m, transaction);

                }

                //回寫 CONTROL_ST
                var paramSoa = new DynamicParameters();
                paramSoa.Add(name: "@trfInvHeaderId", value: trfInvHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                paramSoa.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                paramSoa.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                paramSoa.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);
                Context.Execute(sql: "SP_P222_TrfInvStSummarize", param: paramSoa, transaction: transaction, commandType: CommandType.StoredProcedure);

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


        public async Task<List<long>> GetMiscHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await TrfMiscHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TrfMiscStUpload(long trfMiscHeaderId, MasterUOW masterUOW, string userId = "SYS", IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@trfMiscHeaderId", value: trfMiscHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@processCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@serverCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@batchId", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfMiscStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }

                //換算主單位及交易單位
                string processCode = p.Get<string>("@processCode");
                string serverCode = p.Get<string>("@serverCode");
                string batchId = p.Get<string>("@batchId");

                var list = await SubTransferStRepository.GetListBy(processCode, serverCode, batchId, transaction);
                if (list == null || list.Count == 0)
                {
                    return new ResultModel(false, "TrfMiscStUpload 無資料可上傳");
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var m = list[i];
                    if (m.SECONDARY_QUANTITY > 0 && !string.IsNullOrEmpty(m.SECONDARY_UOM_CODE))
                    {
                        var primaryQuantity = await masterUOW.UomConvertAsync(m.INVENTORY_ITEM_ID, m.SECONDARY_QUANTITY, m.SECONDARY_UOM_CODE, m.PRIMARY_UOM);
                        if (!primaryQuantity.HasValue)
                        {
                            throw new Exception($"單位換算失敗!! ITEM ID :{m.INVENTORY_ITEM_ID} AMOUNT:{m.PRIMARY_UOM} UOM:{m.SECONDARY_UOM_CODE} TO:{m.PRIMARY_UOM}");
                        }

                        m.PRIMARY_QUANTITY = primaryQuantity.Value;
                        m.TRANSACTION_QUANTITY = m.SECONDARY_QUANTITY;
                        m.TRANSACTION_UOM = m.SECONDARY_UOM_CODE;
                    }
                    else
                    {
                        m.TRANSACTION_QUANTITY = m.PRIMARY_QUANTITY;
                        m.TRANSACTION_UOM = m.PRIMARY_UOM;
                        m.ROLL_QUANTITY = Math.Abs(m.PRIMARY_QUANTITY).ToString("0.#####");
                    }
                    await SubTransferStRepository.Update(m, transaction);

                }

                //回寫 CONTROL_ST
                var paramSoa = new DynamicParameters();
                paramSoa.Add(name: "@trfMiscHeaderId", value: trfMiscHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                paramSoa.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                paramSoa.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                paramSoa.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);
                Context.Execute(sql: "SP_P222_TrfMiscStSummarize", param: paramSoa, transaction: transaction, commandType: CommandType.StoredProcedure);

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

        public async Task<List<long>> GetObsHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await TrfObsHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TrfObsStUpload(long trfObsHeaderId, MasterUOW masterUOW, string userId = "SYS", IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@trfObsHeaderId", value: trfObsHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@processCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@serverCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@batchId", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfObsStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
                resultModel = new ResultModel(p.Get<int>("@code"), p.Get<string>("@message"));

                if (!resultModel.Success)
                {
                    return resultModel;
                }


                //換算主單位及交易單位
                string processCode = p.Get<string>("@processCode");
                string serverCode = p.Get<string>("@serverCode");
                string batchId = p.Get<string>("@batchId");

                var list = await SubTransferStRepository.GetListBy(processCode, serverCode, batchId, transaction);
                if (list == null || list.Count == 0)
                {
                    return new ResultModel(false, "TrfMiscStUpload 無資料可上傳");
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var m = list[i];
                    if (m.SECONDARY_QUANTITY > 0 && !string.IsNullOrEmpty(m.SECONDARY_UOM_CODE))
                    {
                        var primaryQuantity = await masterUOW.UomConvertAsync(m.INVENTORY_ITEM_ID, m.SECONDARY_QUANTITY, m.SECONDARY_UOM_CODE, m.PRIMARY_UOM);
                        if (!primaryQuantity.HasValue)
                        {
                            throw new Exception($"單位換算失敗!! ITEM ID :{m.INVENTORY_ITEM_ID} AMOUNT:{m.PRIMARY_UOM} UOM:{m.SECONDARY_UOM_CODE} TO:{m.PRIMARY_UOM}");
                        }

                        m.PRIMARY_QUANTITY = primaryQuantity.Value;
                        m.TRANSACTION_QUANTITY = m.SECONDARY_QUANTITY;
                        m.TRANSACTION_UOM = m.SECONDARY_UOM_CODE;
                    }
                    else
                    {
                        m.TRANSACTION_QUANTITY = m.PRIMARY_QUANTITY;
                        m.TRANSACTION_UOM = m.PRIMARY_UOM;
                        m.ROLL_QUANTITY = Math.Abs(m.PRIMARY_QUANTITY).ToString("0.#####");
                    }
                    await SubTransferStRepository.Update(m, transaction);

                }

                //回寫 CONTROL_ST
                var paramSoa = new DynamicParameters();
                paramSoa.Add(name: "@trfObsHeaderId", value: trfObsHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                paramSoa.Add(name: "@processCode", value: processCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@serverCode", value: serverCode, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@batchId", value: batchId, dbType: DbType.String, direction: ParameterDirection.Input, size: 20);
                paramSoa.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                paramSoa.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                paramSoa.Add(name: "@user", value: userId, dbType: DbType.String, direction: ParameterDirection.Input, size: 128);
                Context.Execute(sql: "SP_P222_TrfMiscStSummarize", param: paramSoa, transaction: transaction, commandType: CommandType.StoredProcedure);

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

        public async Task<List<XXIF_CHP_P222_SUB_TRANSFER_ST>> GetSubTransferStList(string processCode, string serverCode, string batchId, IDbTransaction transaction = null)
        {
            return (await SubTransferStRepository.GetListBy(processCode, serverCode, batchId, transaction));
        }
    }
}
