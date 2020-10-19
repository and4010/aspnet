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
    public class TrfObsHeaderRepository : GenericRepository<TRF_OBSOLETE_HEADER_T>
    {
        
        #region Constructor
        public TrfObsHeaderRepository()
        {
            IdField = "TRANSFER_OBSOLETE_HEADER_ID";
        }

        public TrfObsHeaderRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "TRANSFER_OBSOLETE_HEADER_ID";
        }

        #endregion

        public List<long> GetUploadList(IDbTransaction transaction = null)
        {
            return Connection.Query<long>(
$@"
SELECT H.TRANSFER_OBSOLETE_HEADER_ID FROM TRF_OBSOLETE_HEADER_T H
LEFT JOIN TRF_OBSOLETE_SOA_T S ON S.TRANSFER_OBSOLETE_HEADER_ID = H.TRANSFER_OBSOLETE_HEADER_ID
WHERE S.TRANSFER_OBSOLETE_HEADER_ID IS NULL AND H.NUMBER_STATUS = '1'
", transaction: transaction).ToList();
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
