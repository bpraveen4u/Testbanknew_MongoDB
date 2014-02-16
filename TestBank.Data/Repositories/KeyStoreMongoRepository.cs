using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Data.Infrastructure;
using TestBank.Data.MongoDB;
using TestBank.Entity;
using TestBank.Entity.Models;

namespace TestBank.Data.Repositories
{
    public class KeyStoreMongoRepository : MongoRepositoryBase<ApiKeyStore, string>, IKeyStoreRepository
    {
        public KeyStoreMongoRepository()
            : base()
        {

        }
    }

    public interface IKeyStoreRepository : IRepository<ApiKeyStore, string>
    {
        
    }

}
