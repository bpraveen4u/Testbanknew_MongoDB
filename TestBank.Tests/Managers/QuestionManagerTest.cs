using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestBank.Data.Repositories;
using Moq;
using System.Linq.Expressions;
using TestBank.Entity;
using TestBank.Business.Manager;
using TestBank.Data.Infrastructure;

namespace TestBank.Tests.Managers
{
    [TestClass]
    public class QuestionManagerTest
    {
        private Mock<IQuestionRepository> repository;

        [TestInitialize]
        public void Setup()
        {
            repository = new Mock<IQuestionRepository>();
        }

        [TestMethod]
        public void Get_Questions_Returns_AllQuestions()
        {
            // Arrange   
            InitQuestions(repository);
            var questionManager = new QuestionManager(null, repository.Object);

            //// Act
            var pagedQuestions = questionManager.GetAll(page: 1, pageSize: 3);
            //// Assert
            Assert.IsNotNull(pagedQuestions, "Result is null");
            Assert.IsInstanceOfType(pagedQuestions, typeof(PagedEntity<Question>), "Wrong Model");
            Assert.AreEqual(4, pagedQuestions.TotalRecords, "Wrong number of record count");
            Assert.AreEqual(4, pagedQuestions.PagedData.Count, "Got wrong number of Questions");
        }

        [TestMethod]
        public void Get_Question_Returns_Question()
        {
            // Arrange   
            var fakeQuestion = new Question() { Id = 100, Description = "Test 123 Question", Category = "C#" };
            repository.Setup(s => s.GetByID(100)).Returns(fakeQuestion);

            var questionManager = new QuestionManager(null, repository.Object);

            //// Act
            var question = questionManager.Get(100);
            //// Assert
            Assert.IsNotNull(question, "Result is null");
            Assert.IsInstanceOfType(question, typeof(Question), "Wrong Model");
            Assert.AreEqual(100, question.Id, "Got wrong number of Question");
        }

        [TestMethod]
        public void Insert_Question_Returns_Question()
        {
            // Arrange 
            var fakeUoW = new Mock<IUnitOfWork>();
            fakeUoW.Setup(s => s.Commit());

            var fakeQuestion = new Question() { Id = 100, Description = "Test 123 Question", Category = "C#" };
            repository.Setup(s => s.GetByID(100)).Returns(fakeQuestion);

            var questionManager = new QuestionManager(fakeUoW.Object, repository.Object);

            //// Act
            var newQuestion = questionManager.Insert(fakeQuestion);
            //// Assert
            Assert.IsNotNull(newQuestion, "Result is null");
            Assert.IsInstanceOfType(newQuestion, typeof(Question), "Wrong Model");
            Assert.AreEqual(100, newQuestion.Id, "Got wrong number of Question");
        }


        private void InitQuestions(Mock<IQuestionRepository> questionMock,
            Expression<Func<Question, bool>> filter = null,
            Func<IQueryable<Question>, IOrderedQueryable<Question>> orderBy = null,
            List<Expression<Func<Question, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<Question> fakeAssessments = GetQuestions();
            repository.Setup(x => x.Get(It.IsAny<Expression<Func<Question, bool>>>()
                , It.IsAny<Func<IQueryable<Question>, IOrderedQueryable<Question>>>()
                , It.IsAny<List<Expression<Func<Question, object>>>>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(fakeAssessments);
        }

        private static IQueryable<Question> GetQuestions()
        {
            IQueryable<Question> fakeQuestions = new List<Question> {
                new Question { Id=1, Description="C# Questions 1", Category="C#"},
                new Question { Id=2, Description="C# Questions 2", Category="C#"},
                new Question { Id=3, Description="C# Questions 3", Category="C#"}, 
                new Question { Id=4, Description="C# Questions 4", Category="C#"}
                }.AsQueryable();
            return fakeQuestions;
        }
    }
}
