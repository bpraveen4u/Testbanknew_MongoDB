using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TestBank.Entity;

namespace TestBank.Data.MongoDB
{
    public abstract class MongoRepositoryBase<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// MongoCollection field.
        /// </summary>
        private MongoCollection<TEntity> collection;

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepositoryBase()
            : this(Util<TKey>.GetDefaultConnectionString())
        {
            var conventionPack = new ConventionPack();
            conventionPack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("My Custom Camelcae convention", conventionPack, t => t.FullName.StartsWith("TestBank.Entity"));

            EntityBsonClassMap.Register();
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepositoryBase(string connectionString)
        {
            this.collection = Util<TKey>.GetCollectionFromConnectionString<TEntity>(connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepositoryManager class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepositoryBase(string connectionString, string collectionName)
        {
            this.collection = Util<TKey>.GetCollectionFromConnectionString<TEntity>(connectionString, collectionName);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        public MongoRepositoryBase(MongoUrl url)
        {
            this.collection = Util<TKey>.GetCollectionFromUrl<TEntity>(url);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepositoryBase(MongoUrl url, string collectionName)
        {
            this.collection = Util<TKey>.GetCollectionFromUrl<TEntity>(url, collectionName);
        }

        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        /// for most purposes you can use the MongoRepositoryManager&lt;T&gt;
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        public MongoCollection<TEntity> Collection
        {
            get
            {
                return this.collection;
            }
        }

        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = this.collection.AsQueryable<TEntity>();

            //if (includeProperties != null)
            //    includeProperties.ForEach(i => { query = query.Include(i); });

            //query.Select(includeProperties);

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

        public virtual TEntity GetByID(TKey id)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(TestBank.Entity.Entity)))
            {
                return this.collection.FindOneByIdAs<TEntity>(new ObjectId(id as string));
            }

            return this.collection.FindOneByIdAs<TEntity>(BsonValue.Create(id));
        }

        public virtual void Insert(TEntity entity)
        {
            this.collection.Insert<TEntity>(entity);

            //return entity;
        }

        public virtual void Delete(object id)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(TestBank.Entity.Entity)))
            {
                this.collection.Remove(Query.EQ("_id", new ObjectId(id as string)));
            }
            else
            {
                this.collection.Remove(Query.EQ("_id", BsonValue.Create(id)));
            }
        }

        public virtual void Delete(TKey id)
        {
            if (typeof(TEntity).IsSubclassOf(typeof(TestBank.Entity.Entity)))
            {
                this.collection.Remove(Query.EQ("_id", new ObjectId(id as string)));
            }
            else
            {
                this.collection.Remove(Query.EQ("_id", BsonValue.Create(id)));
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            this.Delete(entityToDelete.Id);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            this.collection.Save<TEntity>(entityToUpdate);
        }
    }
}
