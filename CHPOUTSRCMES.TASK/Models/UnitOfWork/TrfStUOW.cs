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


        public TrfStUOW(IDbConnection conn, bool beginTransaction = false) : base(conn, beginTransaction)
        {
        }

        public async Task<TRF_HEADER_T> GetHeaderById(long headerId, IDbTransaction transaction = null)
        {
            return (await TrfHeaderRepository.GetAsync(headerId, transaction));
        }

        public async Task<List<long>> GetHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await trfHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TransferStUpload(long trfHeaderId, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@trfHeaderId", value: trfHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
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

        public async Task<List<long>> GetReasonHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await trfRsnHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TrfReasonStUpload(long trfRsnHeaderId, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@trfReasonHeaderId", value: trfRsnHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfReasonStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
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


        public async Task<List<long>> GetInvHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await trfRsnHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TrfInvStUpload(long trfInvHeaderId, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@trfInvHeaderId", value: trfInvHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfInvStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
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


        public async Task<List<long>> GetMiscHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await trfMiscHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TrfMiscStUpload(long trfMiscHeaderId, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@trfMiscHeaderId", value: trfMiscHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfMiscStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
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

        public async Task<List<long>> GetObsHeaderListForUpload(IDbTransaction transaction = null)
        {
            return (await trfObsHeaderRepository.GetUploadList(transaction));
        }

        public async Task<ResultModel> TrfObsStUpload(long trfObsHeaderId, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {
                var p = new DynamicParameters();
                p.Add(name: "@trfObsHeaderId", value: trfObsHeaderId, dbType: DbType.Int64, direction: ParameterDirection.Input, size: 20);
                p.Add(name: "@code", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add(name: "@message", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                p.Add(name: "@user", value: "SYS", dbType: DbType.String, direction: ParameterDirection.Input, size: 128);

                Context.Execute(sql: "SP_P222_TrfObsStUpload", param: p, transaction: transaction, commandType: CommandType.StoredProcedure);
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
    }
}
