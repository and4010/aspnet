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
