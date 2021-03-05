using LeaveManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace LeaveManagementSystem.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal ApplicationDbContext Context;
        internal DbSet<T> DbSet;

        public GenericRepository(ApplicationDbContext applicationDbContext)
        {
            this.Context = applicationDbContext;
            this.DbSet = applicationDbContext.Set<T>();
        }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            Expression<Func<T, bool>>[] filters = null
            )
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (filters != null)
            {
                foreach (var expression in filters)
                {
                    if (expression != null)
                    {
                        query = query.Where(expression);
                    }
                }
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
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

        public virtual T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Modified)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}