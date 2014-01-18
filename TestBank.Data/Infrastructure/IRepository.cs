using System;
using System.Linq;
using TestBank.Entity;

namespace TestBank.Data.Infrastructure
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        void Delete(TKey id);
        void Delete(TEntity entityToDelete);
        IQueryable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, System.Collections.Generic.List<System.Linq.Expressions.Expression<Func<TEntity, object>>> includeProperties = null, int? page = null, int? pageSize = null);
        TEntity GetByID(TKey id);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
    }

    //public interface IRepository<TEntity> where TEntity : class, IEntity<int>
    //{
    //}
}
