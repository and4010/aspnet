using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks.Interfaces;
using System.Data.Entity;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private DbContext context = null;

        public DbContext Context
        {
            get { return context;  }
        }

        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 變更儲存資料庫
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            int result = this.Context.SaveChanges();
            return result;
        }

        /// <summary>
        /// 釋放DbContext
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                    this.context = null;
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}