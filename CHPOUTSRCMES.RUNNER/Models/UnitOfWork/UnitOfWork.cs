using System;
using System.Data;
using CHPOUTSRCMES.TASK.Models.Repository;
using CHPOUTSRCMES.TASK.Models.Repository.Interface;
using CHPOUTSRCMES.TASK.Models.UnitOfWork;

namespace BatchPrint.Model.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection context;
        public IDbConnection Context { get { return context; } }
        private IDbTransaction transaction;
        private bool disposed = false;
        private bool enableTransaction = false;

        public UnitOfWork(IDbConnection context, bool beginTransaction = true)
        {
            this.context = context;
            if (this.context.State != ConnectionState.Open
                || this.context.State != ConnectionState.Connecting)
                this.context.Open();

            if (beginTransaction) BeginTransaction();
            
        }


        public void BeginTransaction()
        {
            enableTransaction = true;
            transaction = enableTransaction ? this.context.BeginTransaction() : null;
        }

        public void Commit()
        {
            try
            {
                if (enableTransaction && transaction != null)
                    transaction.Commit();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (enableTransaction && transaction != null)
                {
                    transaction.Dispose();
                    transaction = context.BeginTransaction();
                }
            }
        }

        public void Rollback()
        {
            try
            {
                if (enableTransaction && transaction != null)
                    transaction.Rollback();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (enableTransaction && transaction != null)
                {
                    transaction.Dispose();
                    transaction = context.BeginTransaction();
                }
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (enableTransaction && transaction != null)
                    {
                        transaction.Dispose();
                        transaction = null;
                    }
                    if(context != null) {
                        if(context.State ==  ConnectionState.Open)
                            context.Close();
                        context.Dispose();
                        context = null;
                    }
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
