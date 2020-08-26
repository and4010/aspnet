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
    public class CtrStRepository : GenericRepository<XXIF_CHP_P217_CONTAINER_ST>
    {
        #region Constructor
        public CtrStRepository()
        {

        }

        public CtrStRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
        }

        #endregion

        public async Task<XXIF_CHP_P217_CONTAINER_ST> GetSingleBy(string processCode, string serverCode, string batchId, IDbTransaction transaction = null)
        {
            return await Connection.QuerySingleAsync<XXIF_CHP_P217_CONTAINER_ST>(
@"SELECT TOP 1 * FROM XXIF_CHP_P217_CONTAINER_ST A 
WHERE A.PROCESS_CODE = @ProcessCode AND A.SERVER_CODE = @ServerCode AND A.BATCH_ID = @BatchId
ORDER BY A.PROCESS_CODE, A.SERVER_CODE, A.BATCH_ID, A.BATCH_LINE_ID", new { ProcessCode = processCode, ServerCode = serverCode, BatchId = batchId }, transaction: transaction);
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
