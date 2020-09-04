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
    public class CtrHeaderRepository : GenericRepository<CTR_HEADER_T>
    {
        
        #region Constructor
        public CtrHeaderRepository()
        {
            IdField = "CTR_HEADER_ID";
        }

        public CtrHeaderRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "CTR_HEADER_ID";
        }

        #endregion

        public async Task<List<CTR_HEADER_T>> GetUploadList(IDbTransaction transaction = null)
        {
            return (await Connection.QueryAsync<CTR_HEADER_T>(
$@"SELECT t.* FROM {tableName} t 
LEFT JOIN CTR_SOA_T s ON s.{IdField} = t.{IdField}
WHERE s.{IdField} IS NULL AND t.STATUS =0
", transaction: transaction)).ToList();
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
