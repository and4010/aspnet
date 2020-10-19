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
    public class TrfHeaderRepository : GenericRepository<TRF_HEADER_T>
    {
        
        #region Constructor
        public TrfHeaderRepository()
        {
            IdField = "TRANSFER_HEADER_ID";
        }

        public TrfHeaderRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "TRANSFER_HEADER_ID";
        }

        #endregion

        public List<long> GetUploadList(IDbTransaction transaction = null)
        {
            //取得上傳清單，排除已上傳、未存檔及 倉庫移轉MES入庫
            return Connection.Query<long>(
$@"
SELECT H.TRANSFER_HEADER_ID FROM TRF_HEADER_T H
LEFT JOIN TRF_SOA_T S ON S.TRANSFER_HEADER_ID = H.TRANSFER_HEADER_ID AND S.TRANSFER_TYPE = H.TRANSFER_TYPE
WHERE S.TRANSFER_HEADER_ID IS NULL AND H.NUMBER_STATUS = '1' AND NOT (H.IS_MES = '1' AND H.TRANSFER_CATALOG = 'INV' AND H.TRANSFER_TYPE = 'I')
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
