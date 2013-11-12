using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;
using TestBank.Data.Infrastructure;

namespace TestBank.Data.Repositories
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }

    public interface IQuestionRepository : IRepository<Question>
    {

    }
}
