using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Data.Infrastructure;
using TestBank.Data.MongoDB;
using TestBank.Entity;

namespace TestBank.Data.Repositories
{
    public class UserAnswerMongoRepository : MongoRepositoryBase<UserAnswer, int>, IUserAnswerRepository
    {
        public UserAnswerMongoRepository()
            : base()
        {

        }
    }

    public interface IUserAnswerRepository : IRepository<UserAnswer, int>
    {
        
    }

}
