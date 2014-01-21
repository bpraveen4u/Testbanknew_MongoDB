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
using ExpectedObjects;
using ExpectedObjects.Comparisons;
using ExpectedObjects.Strategies;
//using Machine.Specifications;

namespace TestBank.Tests.Managers
{
    [TestClass]
    public class QuestionManagerTest
    {
        private Mock<IQuestionRepository> repository;
        private Mock<IUnitOfWork> fakeUoW;
        private QuestionManager questionManager;
        [TestInitialize]
        public void Setup()
        {
            repository = new Mock<IQuestionRepository>();
            fakeUoW = new Mock<IUnitOfWork>();
        }

        [TestMethod]
        public void GetAll_QuestionManager_Returns_PagedQuestions()
        {
            // Arrange   
            InitQuestions(repository);
            questionManager = new QuestionManager(null, repository.Object);

            //// Act
            var pagedQuestions = questionManager.GetAll(page: 1, pageSize: 3);
            //// Assert
            Assert.IsNotNull(pagedQuestions, "Result is null");
            Assert.IsInstanceOfType(pagedQuestions, typeof(PagedEntity<Question>), "Wrong Model");
            Assert.AreEqual(4, pagedQuestions.TotalRecords, "Wrong number of record count");
            Assert.AreEqual(4, pagedQuestions.PagedData.Count, "Got wrong number of Questions");
        }

        [TestMethod]
        public void Get_QuestionManager_Returns_Question()
        {
            // Arrange   
            var fakeQuestion = new Question() { Id = 100, Description = "Test 123 Question", Category = "C#" };
            repository.Setup(s => s.GetByID(100)).Returns(fakeQuestion);

            questionManager = new QuestionManager(null, repository.Object);

            //// Act
            var question = questionManager.Get(100);
            //// Assert
            Assert.IsNotNull(question, "Result is null");
            Assert.IsInstanceOfType(question, typeof(Question), "Wrong Model");
            Assert.AreEqual(100, question.Id, "Got wrong number of Question");
        }

        [TestMethod]
        public void Insert_QuestionManager_Returns_Question()
        {
            // Arrange 
            var fakeUoW = new Mock<IUnitOfWork>();
            fakeUoW.Setup(s => s.Commit());

            var fakeQuestion = GetFakeQuestion();
            repository.Setup(s => s.GetByID(1000)).Returns(fakeQuestion);

            questionManager = new QuestionManager(fakeUoW.Object, repository.Object);

            //// Act
            var newQuestion = questionManager.Insert(fakeQuestion);
            //// Assert
            Assert.IsNotNull(newQuestion, "Result is null");
            Assert.IsInstanceOfType(newQuestion, typeof(Question), "Wrong Model");
            Assert.AreEqual(1000, newQuestion.Id, "Got wrong number of Question");
        }

        private static Question GetFakeQuestion()
        {
            var fakeQuestion = new Question()
            {
                Id = 1000,
                Description = "Test 123 Question",
                Category = "C#",
                Options = new List<Option>() { new Option() { Id = "A", Description = "Opt A", Type = OptionType.RadioButton, IsCorrect = false},
                    new Option() { Id = "B", Description = "Opt B", Type = OptionType.RadioButton, IsCorrect = true}
                },
                CorrectScore = 1
            };
            return fakeQuestion;
        }

        [TestMethod]
        public void Update_QuestionManager_Returns_Question()
        {
            // Arrange   
            var fakeQuestion = GetFakeQuestion();
            repository.Setup(x => x.Update(It.IsAny<Question>()));
            questionManager = new QuestionManager(fakeUoW.Object, repository.Object);
            //// Act
            var newQuestion = questionManager.Update(fakeQuestion);

            //// Assert
            Assert.IsNotNull(newQuestion, "Result is null");
            Assert.IsInstanceOfType(newQuestion, typeof(Question), "Invalid Enitity");
            Assert.AreEqual(1000, newQuestion.Id);
        }

        [TestMethod]
        public void Delete_QuestionManager_Returns_void()
        {
            // Arrange   
            var fakeQuestion = new Question() { Id = 1000, Description = "test fake assessment", Category = "C" };
            repository.Setup(x => x.Delete(1000));
            questionManager = new QuestionManager(fakeUoW.Object, repository.Object);
            //// Act
            questionManager.Delete(1000);

            //// Assert
            //Assert.IsNotNull(newAssessment, "Result is null");
            //Assert.IsInstanceOfType(newAssessment, typeof(Assessment), "Invalid Enitity");
            //Assert.AreEqual(1000, newAssessment.Id);
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

    //public class when_the_customer_requests_product_information
    //{
        
    //    Establish context = () =>
    //    {
    //        _mockedRepository = new Mock<IRepository>();
    //        _accountCreator = new AccountCreator(_mockedRepository.Object);

    //        _newAccount = new NewAccount();
    //        _account = new Account();

    //        _mockedRepository
    //            .Setup(x => x.Create(Moq.It.IsAny<Account>()))
    //            .Returns(_account);
    //    };
    //}
}
