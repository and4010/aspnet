using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CHPOUTSRCMES.TASK.Models.Entity;
using Dapper;


namespace CHPOUTSRCMES.TASK.Models.Repository.MsSql
{
    public class OspSoaS3Repository : GenericRepository<OSP_SOA_S3_T>
    {
        
        #region Constructor
        public OspSoaS3Repository()
        {
            IdField = "";
        }

        public OspSoaS3Repository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "";
        }

        #endregion

        public async Task<List<long>> GetUploadList(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<long>(
$@"
SELECT O.OSP_HEADER_ID FROM OSP_HEADER_T O 
LEFT JOIN OSP_SOA_S2_T S2 ON S2.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_SOA_S3_T S3 ON S3.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] IN ('3', '4') 
AND S2.STATUS_CODE = 'S'
AND (S3.OSP_HEADER_ID IS NULL OR S3.STATUS_CODE = 'R')
", transaction: transaction)).ToList();
        }

        public async Task<List<OSP_SOA_S3_T>> GetUploadedList(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<OSP_SOA_S3_T>(
$@"
SELECT S2.* FROM OSP_HEADER_T O 
JOIN OSP_SOA_S3_T S2 ON S2.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] IN ('3', '4') AND S2.STATUS_CODE IS NULL 
", transaction: transaction)).ToList();
        }

        public async Task<ResultModel> UpdateStatusCode(OSP_SOA_S3_T data, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {

                int count = await Connection.ExecuteAsync(
$@"UPDATE OSP_SOA_S3_T SET 
    STATUS_CODE=@status, LAST_UPDATE_BY = @user, LAST_UPDATE_DATE = @createDate 
    WHERE OSP_HEADER_ID = @ospHeaderId 
    AND PROCESS_CODE = @processCode 
    AND SERVER_CODE = @serverCode 
    AND BATCH_ID = @batchId", new
{
    ospHeaderId = data.OSP_HEADER_ID,
    processCode = data.PROCESS_CODE,
    serverCode = data.SERVER_CODE,
    batchId = data.BATCH_ID,
    status = data.STATUS_CODE,
    user = data.LAST_UPDATE_BY,
    createDate = data.LAST_UPDATE_DATE
}, transaction);
                resultModel.Code = 0;
                resultModel.Success = true;
                resultModel.Msg = "";
            }
            catch(Exception ex)
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
