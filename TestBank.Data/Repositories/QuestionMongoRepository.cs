﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;
using TestBank.Data.Infrastructure;
using TestBank.Data.MongoDB;
using MongoDB.Driver.Linq;

namespace TestBank.Data.Repositories
{
    public class QuestionMongoRepository : MongoRepositoryBase<Question, int>, IQuestionRepository
    {
        public QuestionMongoRepository()
            : base()
        {

        }

        public List<Question> GetAll(string category)
        {
            var query = Collection.AsQueryable<Question>();
            return query.Where(q => q.Category.ToLower() == category.ToLower()).Select(q => q).ToList();
        }
    }
}
