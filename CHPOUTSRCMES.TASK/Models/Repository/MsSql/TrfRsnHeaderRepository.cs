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
    public class TrfRsnHeaderRepository : GenericRepository<TRF_REASON_HEADER_T>
    {
        
        #region Constructor
        public TrfRsnHeaderRepository()
        {
            IdField = "TRF_REASON_HEADER_ID";
        }

        public TrfRsnHeaderRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "TRF_REASON_HEADER_ID";
        }

        #endregion

        public async Task<List<long>> GetUploadList(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<long>(
$@"
SELECT H.TRANSFER_REASON_HEADER_ID FROM TRF_REASON_HEADER_T H
LEFT JOIN TRF_REASON_SOA_T S ON S.TRANSFER_REASON_SOA_ID = H.TRANSFER_REASON_HEADER_ID
WHERE S.TRANSFER_REASON_SOA_ID IS NULL AND H.NUMBER_STATUS = '1'
", transaction: transaction)).ToList();
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
