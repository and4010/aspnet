using System;
using System.Data;
using System.Data.Common;

namespace CHPOUTSRCMES.TASK.Models.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Context { get; }
        void Commit();
        void Rollback();
    }
}
