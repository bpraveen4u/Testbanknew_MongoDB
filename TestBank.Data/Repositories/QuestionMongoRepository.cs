using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;
using TestBank.Data.Infrastructure;
using TestBank.Data.MongoDB;

namespace TestBank.Data.Repositories
{
    public class QuestionMongoRepository : MongoRepositoryBase<Question, int>, IQuestionRepository
    {
        public QuestionMongoRepository()
            : base()
        {

        }
    }
}
