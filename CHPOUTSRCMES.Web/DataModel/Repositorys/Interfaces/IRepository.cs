using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CHPOUTSRCMES.Web.DataModel.Entity.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Create(T instance, bool saveChanged = false);


        void Update(T instance, bool saveChanged = false);


        void Update(T instance, bool saveChanged = false, params object[] keyValues);


        void Delete(T instance, bool saveChanged = false);

        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        IEnumerable<T> Query(string queryString, params object[] parameters);

        IQueryable<T> GetAll();

        void SaveChanges();

    }
}