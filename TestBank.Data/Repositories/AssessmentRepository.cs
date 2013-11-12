using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Data.Infrastructure;
using TestBank.Entity;

namespace TestBank.Data.Repositories
{
    public class AssessmentRepository : RepositoryBase<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<Assessment> GetAssessmentWithQuetions()
        {
            //this.Get()
            throw new NotImplementedException();
        }
    }

    public interface IAssessmentRepository : IRepository<Assessment>
    {
        IEnumerable<Assessment> GetAssessmentWithQuetions();
    }
}
