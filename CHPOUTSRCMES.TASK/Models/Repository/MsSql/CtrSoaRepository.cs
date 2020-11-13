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
    public class CtrSoaRepository : GenericRepository<CTR_SOA_T>
    {
        
        #region Constructor
        public CtrSoaRepository()
        {
            IdField = "";
        }

        public CtrSoaRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "";
        }

        #endregion

        public List<CTR_SOA_T> GetUploadedList(IDbTransaction transaction = null)
        {
            return Connection.Query<CTR_SOA_T>(
$@"
SELECT S.* FROM CTR_HEADER_T O 
JOIN CTR_SOA_T S ON S.CTR_HEADER_ID = O.CTR_HEADER_ID
WHERE O.[STATUS] IN ('0')
AND (S.STATUS_CODE IS NULL OR S.STATUS_CODE = 'E')
", transaction: transaction).ToList();
        }

        public ResultModel UpdateStatusCode(CTR_SOA_T data, IDbTransaction transaction = null)
        {
            var resultModel = new ResultModel();

            try
            {

                int count = Connection.Execute(
$@"UPDATE CTR_SOA_T SET 
    STATUS_CODE=@status, LAST_UPDATE_BY = @user, LAST_UPDATE_DATE = @createDate 
    WHERE CTR_HEADER_ID = @ctrHeaderId 
    AND PROCESS_CODE = @processCode 
    AND SERVER_CODE = @serverCode 
    AND BATCH_ID = @batchId", new
{
    ctrHeaderId = data.CTR_HEADER_ID,
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
