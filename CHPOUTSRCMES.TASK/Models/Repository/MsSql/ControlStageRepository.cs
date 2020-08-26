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
