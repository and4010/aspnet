using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CHPOUTSRCMES.TASK.Models.Entity;
using Dapper;


namespace CHPOUTSRCMES.TASK.Models.Repository.MsSql
{
    public class ControlStageRepository : GenericRepository<XXIF_CHP_CONTROL_ST>
    {
        #region Constructor
        public ControlStageRepository()
        {

        }

        public ControlStageRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
        }

        #endregion

        public async Task<IEnumerable<XXIF_CHP_CONTROL_ST>> GetControlStageListBy(string processCode, string soaProcessCode = "S", string pullingFlag = "Out-S", IDbTransaction transaction = null)
        {
            return await this.Connection.QueryAsync<XXIF_CHP_CONTROL_ST>(
$@"{GenerateSelectQuery()} A 
WHERE A.PROCESS_CODE = @ProcessCode AND A.SOA_PROCESS_CODE = @SoaProcessCode AND A.SOA_PULLING_FLAG = @PullingFlag
ORDER BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID", new { ProcessCode = processCode, SoaProcessCode = soaProcessCode, PullingFlag = pullingFlag },transaction: transaction);
        }

        public async Task<XXIF_CHP_CONTROL_ST> GetBy(string processCode, string serverCode, string batchId, string pullingFlag = "In-S", IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<XXIF_CHP_CONTROL_ST>(
$@"{GenerateSelectQuery()} A 
WHERE A.PROCESS_CODE = @ProcessCode AND A.SERVER_CODE = @ServerCode AND A.BATCH_ID = @BatchId AND SOA_PULLING_FLAG=@PullingFlag
ORDER BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID", new { ProcessCode = processCode, ServerCode = serverCode, BatchId = batchId, PullingFlag = pullingFlag }, transaction: transaction)).FirstOrDefault();
        }

        public async Task<ResultModel> UpdateStatus(XXIF_CHP_CONTROL_ST st, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {

                int count = await Connection.ExecuteAsync(
$@"UPDATE XXIF_CHP_CONTROL_ST SET 
    STATUS_CODE=@statusCode, ERROR_MSG=@errorMsg, SOA_PULLING_FLAG=@soaPullingFlag
    WHERE PROCESS_CODE = @processCode 
    AND SERVER_CODE = @serverCode 
    AND BATCH_ID = @batchId", new
{
    processCode = st.PROCESS_CODE,
    serverCode = st.SERVER_CODE,
    batchId = st.BATCH_ID,
    statusCode = st.STATUS_CODE,
    errorMsg = st.ERROR_MSG,
    soaPullingFlag = st.SOA_PULLING_FLAG
}, transaction: transaction);
                resultModel.Code = 0;
                resultModel.Success = true;
                resultModel.Msg = "";
            }
            catch (Exception ex)
            {
                resultModel.Code = -99;
                resultModel.Success = false;
                resultModel.Msg = ex.Message;
            }
            return resultModel;

        }

        #region IDispose Region
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
