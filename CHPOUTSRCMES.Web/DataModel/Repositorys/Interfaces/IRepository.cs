using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {

        DbContext getContext();

        void Create(TEntity instance);

        void Update(TEntity instance);

        void Update(TEntity instance, params object[] keyValues);

        void Update(TEntity instance, bool saveChanged, params object[] keyValues);

        void Delete(TEntity instance);

        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        void SaveChanges();



    }
}