using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Data.Infrastructure;
using TestBank.Data.MongoDB;
using TestBank.Entity;

namespace TestBank.Data.Repositories
{
    public class UserMongoRepository : MongoRepositoryBase<User, string>, IUserRepository
    {
        public UserMongoRepository()
            : base()
        {

        }
    }

    public interface IUserRepository : IRepository<User, string>
    {
        
    }
}
