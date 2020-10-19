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
    public class OspBatchStRepository : GenericRepository<XXIF_CHP_P219_OSP_BATCH_ST>
    {
        
        #region Constructor
        public OspBatchStRepository()
        {
            IdField = "XXIF_CHP_P219_OSP_BATCH_ST";
        }

        public OspBatchStRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "XXIF_CHP_P219_OSP_BATCH_ST";
        }

        #endregion

        public List<XXIF_CHP_P219_OSP_BATCH_ST> GetListBy(string processCode, string serverCode, string batchId, IDbTransaction transaction = null)
        {
            return Connection.Query<XXIF_CHP_P219_OSP_BATCH_ST>(
@"SELECT * FROM XXIF_CHP_P219_OSP_BATCH_ST A 
WHERE A.PROCESS_CODE = @ProcessCode AND A.SERVER_CODE = @ServerCode AND A.BATCH_ID = @BatchId
ORDER BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID, A.BATCH_LINE_ID", new { ProcessCode = processCode, ServerCode = serverCode, BatchId = batchId }, transaction: transaction).ToList();
        }

        public ResultModel UpdateStatus(XXIF_CHP_P219_OSP_BATCH_ST st, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {

                int count = Connection.Execute(
$@"UPDATE XXIF_CHP_P219_OSP_BATCH_ST SET 
    STATUS_CODE=@statusCode, ERROR_MSG=@errorMsg
    WHERE
    PROCESS_CODE = @processCode 
    AND SERVER_CODE = @serverCode 
    AND BATCH_ID = @batchId
    AND BATCH_LINE_ID = @batchLineId", new
{
    processCode = st.PROCESS_CODE,
    serverCode = st.SERVER_CODE,
    batchId = st.BATCH_ID,
    batchLineId = st.BATCH_LINE_ID,
    statusCode = st.STATUS_CODE,
    errorMsg = st.ERROR_MSG
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

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
            }
            this.disposed = true;
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
