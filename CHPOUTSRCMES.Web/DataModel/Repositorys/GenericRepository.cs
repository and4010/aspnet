using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using EntityFramework.Utilities;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {

        private DbContext _context { get; set; }


        public DbContext getContext()
        {
            return this._context;
        }


        public GenericRepository() : this(new MesContext())
        {
        }


        public GenericRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._context = context;
        }


        public GenericRepository(ObjectContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._context = new DbContext(context, true);
        }


        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="System.ArgumentNullException">instance</exception>
        public void Create(TEntity instance, bool saveChanged = false)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");

            }

            this._context.Set<TEntity>().Add(instance);

            if (saveChanged)
            {
                this.SaveChanges();
            }

        }

        ///// <summary>
        ///// Creates instances at once.
        ///// </summary>
        ///// <param name="instance">The instance.</param>
        ///// <exception cref="System.ArgumentNullException">instance</exception>
        //public void Create(IEnumerable<TEntity> instance)
        //{
        //    EFBatchOperation.For(_context, _context.Set<TEntity>()).InsertAll(instance);
        //}


        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Update(TEntity instance, bool savedChanged = false)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            this._context.Entry(instance).State = EntityState.Modified;
            if (savedChanged)
            {
                this.SaveChanges();
            }
        }


        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="keyValues">The key values.</param>
        /// <exception cref="System.ArgumentNullException">instance</exception>
        public void Update(TEntity instance, bool saveChanged, params object[] keyValues)
        {

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var entry = _context.Entry<TEntity>(instance);
            if (entry.State == EntityState.Detached)
            {
                var set = _context.Set<TEntity>();
                TEntity attachedEntity = set.Find(keyValues);
                if (attachedEntity != null)
                {
                    var attachedEntry = _context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(instance);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
            }

            if (saveChanged)
            {
                this.SaveChanges();
            }
        }


        /// <summary>
        /// Deletes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(TEntity instance, bool saveChanged = false)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            this._context.Entry(instance).State = EntityState.Deleted;
            if (saveChanged)
            {
                this.SaveChanges();
            }
        }


        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this._context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {

                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).AsNoTracking().ToList();
            }
            else
            {
                return query.AsNoTracking().ToList();
            }

        }


        public IEnumerable<TEntity> Query(string queryString, params object[] parameters)
        {
            return _context.Set<TEntity>().SqlQuery(queryString, parameters).AsNoTracking().ToList();
        }


        public void SaveChanges()
        {
            this._context.SaveChanges();
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }
    }
}