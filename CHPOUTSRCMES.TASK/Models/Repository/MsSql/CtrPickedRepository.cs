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
    public class CtrPickedRepository : GenericRepository<CTR_PICKED_T>
    {
        
        #region Constructor
        public CtrPickedRepository()
        {
            IdField = "CTR_PICKED_ID";
        }

        public CtrPickedRepository(IDbConnection conn, string tableName) :base(conn, tableName)
        {
            IdField = "CTR_PICKED_ID";
        }

        #endregion


        public List<CTR_DETAIL_T> GetListByHeaderId(long headerId, IDbTransaction transaction = null)
        {
            return Connection.Query<CTR_DETAIL_T>($"{GenerateSelectQuery()} WHERE CTR_HEADER_ID = @headerId", new { headerId = headerId }, transaction: transaction).ToList();
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
