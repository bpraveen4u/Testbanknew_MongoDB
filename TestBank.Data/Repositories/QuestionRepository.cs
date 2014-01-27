using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;
using TestBank.Data.Infrastructure;

namespace TestBank.Data.Repositories
{
    public class QuestionRepository : RepositoryBase<Question, int>, IQuestionRepository
    {
        public QuestionRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
            
        }

        public List<Question> GetAll(string category)
        {
            throw new NotImplementedException();
        }
    }

    public interface IQuestionRepository : IRepository<Question, int>
    {
        List<Question> GetAll(string category);
    }
}
