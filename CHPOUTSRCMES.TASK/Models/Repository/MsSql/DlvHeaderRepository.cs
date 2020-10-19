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
    public class DlvHeaderRepository : GenericRepository<DLV_HEADER_T>
    {
        
        #region Constructor
        public DlvHeaderRepository()
        {
            IdField = "DLV_HEADER_ID";
        }

        public DlvHeaderRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "DLV_HEADER_ID";
        }

        #endregion

        public List<long> GetUploadList(IDbTransaction transaction = null)
        {
            return Connection.Query<long>(
$@"
SELECT H.TRIP_ID FROM DLV_HEADER_T H
LEFT JOIN DLV_SOA_T S ON S.TRIP_ID = H.TRIP_ID
WHERE S.TRIP_ID IS NULL 
GROUP BY H.TRIP_ID
HAVING MIN(H.DELIVERY_STATUS_CODE) = 'DH5'
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
