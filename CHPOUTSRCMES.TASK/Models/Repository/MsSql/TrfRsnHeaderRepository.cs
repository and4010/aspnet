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

        public List<long> GetUploadList(IDbTransaction transaction = null)
        {
            //入庫貨故移轉上傳，必須等待20分後始可上傳 避免搶先入庫上傳
            return Connection.Query<long>(
$@"
SELECT TRANSFER_REASON_HEADER_ID FROM (
SELECT H.TRANSFER_REASON_HEADER_ID FROM TRF_REASON_HEADER_T H
LEFT JOIN TRF_REASON_SOA_T S ON S.TRANSFER_REASON_HEADER_ID = H.TRANSFER_REASON_HEADER_ID
LEFT JOIN CTR_HEADER_T C ON C.CTR_HEADER_ID = H.CTR_HEADER_ID
LEFT JOIN CTR_SOA_T A ON A.CTR_HEADER_ID = C.CTR_HEADER_ID
WHERE H.NUMBER_STATUS = '1'
AND S.TRANSFER_REASON_HEADER_ID IS NULL  
AND C.CTR_HEADER_ID IS NOT NULL 
AND A.STATUS_CODE ='S'
UNION 
SELECT H.TRANSFER_REASON_HEADER_ID FROM TRF_REASON_HEADER_T H
LEFT JOIN TRF_REASON_SOA_T S ON S.TRANSFER_REASON_HEADER_ID = H.TRANSFER_REASON_HEADER_ID
WHERE S.TRANSFER_REASON_HEADER_ID IS NULL AND H.NUMBER_STATUS = '1'  AND H.CTR_HEADER_ID IS NULL
) A
ORDER BY TRANSFER_REASON_HEADER_ID
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
