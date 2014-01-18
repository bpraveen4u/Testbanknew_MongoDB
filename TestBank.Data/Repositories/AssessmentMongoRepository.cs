using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Data.Infrastructure;
using TestBank.Data.MongoDB;
using TestBank.Entity;

namespace TestBank.Data.Repositories
{
    public class AssessmentMongoRepository : MongoRepositoryBase<Assessment, int>, IAssessmentRepository
    {
        public AssessmentMongoRepository()
            : base()
        {

        }

        public IEnumerable<Assessment> GetAssessmentWithQuetions()
        {
            //this.Get()
            throw new NotImplementedException();
        }
    }

}
