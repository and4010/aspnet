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
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks.Interfaces;

namespace CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public IUnitOfWork UnitOfWork { get; set; }

        private DbContext context { set; get; }

        public GenericRepository(IUnitOfWork uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException("uow");
            }
            this.UnitOfWork = uow;
            this.context = uow.Context;
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

            this.context.Set<TEntity>().Add(instance);

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

            this.context.Entry<TEntity>(instance).State = EntityState.Modified;
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

            var entry = this.context.Entry(instance);
            if (entry.State == EntityState.Detached)
            {
                var set = this.context.Set<TEntity>();
                TEntity attachedEntity = set.Find(keyValues);
                if (attachedEntity != null)
                {
                    var attachedEntry = this.context.Entry(attachedEntity);
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
            this.context.Entry(instance).State = EntityState.Deleted;
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
            IQueryable<TEntity> query = this.context.Set<TEntity>();

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
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }

        }


        public IEnumerable<TEntity> Query(string queryString, params object[] parameters)
        {
            return this.context.Set<TEntity>().SqlQuery(queryString, parameters).ToList();
        }


        public IQueryable<TEntity> GetAll()
        {
            return context.Set<TEntity>().AsQueryable();
        }


        public void SaveChanges()
        {
            this.context.SaveChanges();
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
                if (this.context != null)
                {
                    this.context.Dispose();
                    this.context = null;
                }
            }
        }
    }
}