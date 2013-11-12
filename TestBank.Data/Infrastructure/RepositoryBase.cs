using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;
using TestBank.Entity;

namespace TestBank.Data.Infrastructure
{
    public abstract class RepositoryBase<TEntity> where TEntity : class, IEntity
    {
        internal TestBankContext dataContext;
        internal IDbSet<TEntity> dbSet;

        public RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            this.dbSet = DataContext.Set<TEntity>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected TestBankContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => { query = query.Include(i); });

            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            if (orderBy != null)
            {
                return orderBy(query);
            }

            if (page != null && pageSize != null)
            {
                if (orderBy == null)
                {
                    query = query.OrderBy(t => t.Id).Skip((page.Value - 1) * pageSize.Value)
                                 .Take(pageSize.Value);
                }
                else
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value)
                                 .Take(pageSize.Value);
                }
            }
            
            return query;
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (dataContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            dataContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
