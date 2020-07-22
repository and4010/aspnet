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


        void Create(TEntity instance, bool saveChanged = false);


        void Update(TEntity instance, bool saveChanged = false);


        void Update(TEntity instance, bool saveChanged = false, params object[] keyValues);


        void Delete(TEntity instance, bool saveChanged = false);


        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        IEnumerable<TEntity> Query(string queryString, params object[] parameters);


        void SaveChanges();

    }
}