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
    public class OspSoaS1Repository : GenericRepository<OSP_SOA_S1_T>
    {
        
        #region Constructor
        public OspSoaS1Repository()
        {
            IdField = "";
        }

        public OspSoaS1Repository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "";
        }

        #endregion

        public List<long> GetUploadList(IDbTransaction transaction = null)
        {
            return Connection.Query<long>(
$@"
SELECT O.OSP_HEADER_ID FROM OSP_HEADER_T O 
LEFT JOIN OSP_SOA_S1_T S1 ON S1.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_HEADER_MOD_T M ON M.OSP_HEADER_ID = O.OSP_HEADER_ID
LEFT JOIN OSP_SOA_S3_T S3 ON S3.OSP_HEADER_ID = M.ORG_OSP_HEADER_ID 
WHERE O.[STATUS] IN ('3', '4', '5')
AND S1.OSP_HEADER_ID IS NULL
AND ( M.ORG_OSP_HEADER_ID IS NULL OR ( M.ORG_OSP_HEADER_ID IS NOT NULL AND S3.STATUS_CODE IN ('S', 'X') ))
", transaction: transaction).ToList();
        }

        public List<OSP_SOA_S1_T> GetUploadedList(IDbTransaction transaction = null)
        {
            return Connection.Query<OSP_SOA_S1_T>(
$@"
SELECT S1.* FROM OSP_HEADER_T O 
JOIN OSP_SOA_S1_T S1 ON S1.OSP_HEADER_ID = O.OSP_HEADER_ID
WHERE O.[STATUS] IN ('3', '4', '5')
AND (S1.STATUS_CODE IS NULL OR S1.STATUS_CODE = 'E')
", transaction: transaction).ToList();
        }

        public ResultModel UpdateStatusCode(OSP_SOA_S1_T data, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {

                int count = Connection.Execute(
$@"UPDATE OSP_SOA_S1_T SET 
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
}, transaction: transaction);
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
