using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestBank.Data.Repositories;
using Moq;
using TestBank.Entity;
using TestBank.API.WebHost.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using TestBank.API.WebHost.Controllers;
using TestBank.Business.Manager;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using TestBank.API.WebHost;
using System.Web.Http.Routing;
using AutoMapper;
using System.Net;
using Newtonsoft.Json;
using TestBank.Data.Infrastructure;
using TestBank.API.WebHost.Infrastructure.Filters;
using System.Web.Http.Controllers;
using TestBank.Business.Exceptions;

namespace TestBank.Tests.ApiControllers
{
    [TestClass]
    public class QuestionApiControllerTest
    {
        private Mock<IQuestionRepository> questionRepository;
        private QuestionManager questionManager;
        private Mock<IUnitOfWork> fakeUoW;
        [TestInitialize]
        public void Setup()
        {
            questionRepository = new Mock<IQuestionRepository>();
            fakeUoW = new Mock<IUnitOfWork>();
            fakeUoW.Setup(u => u.Commit());
        }

        [TestMethod]
        public void Get_All_Returns_AllQuestions_Action()
        {
            // Arrange   
            IQueryable<Question> fakeQuestion = GetQuestions();
            questionRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Question, bool>>>()
                ,It.IsAny<Func<IQueryable<Question>, IOrderedQueryable<Question>>>()
                , It.IsAny<List<Expression<Func<Question, object>>>>(),It.IsAny<int?>(), It.IsAny<int?>())).Returns(fakeQuestion);
            questionManager = new QuestionManager(fakeUoW.Object, questionRepository.Object);
            var controller = SetupControllerContext(HttpMethod.Get, "http://localhost/api/questions/");

            //// Act
            var pagedQuestions = controller.GetAll();
            //// Assert
            Assert.IsNotNull(pagedQuestions, "Result is null");
            Assert.IsInstanceOfType(pagedQuestions, typeof(PagedModel<QuestionModel>), "Wrong Model");
            Assert.AreEqual(4, pagedQuestions.TotalRecords, "Wrong number of record count");
            Assert.AreEqual(4, pagedQuestions.PagedData.Count, "Got wrong number of Questions");
        }

        private QuestionsController SetupControllerContext(HttpMethod method, string url)
        {
            Mapper.CreateMap<Question, QuestionModel>();
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Filters.Add(new BusinessExceptionAttribute());
            
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["Questions"],
                new HttpRouteValueDictionary { { "controller", "Questions" } });
            var controller = new QuestionsController(questionManager)
            {
                Request = new HttpRequestMessage(method, url)
                {
                    Properties = 
                    {
                        { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                        { HttpPropertyKeys.HttpRouteDataKey, httpRouteData } 
                    }
                }
                , ControllerContext = ContextUtil.CreateControllerContext()
                
            };
            return controller;
        }
        
        [TestMethod]
        public void Get_One_Question_Action()
        {
            // Arrange   
            Question fakeQuestion = new Question() { Id = 1000, Description = "test fake assessment" };
            questionRepository.Setup(x => x.GetByID(1000)).Returns(fakeQuestion);

            questionManager = new QuestionManager(fakeUoW.Object, questionRepository.Object);
            var controller = SetupControllerContext(HttpMethod.Get, "http://localhost/api/questions/1000");
            //// Act
            var response = controller.Get(1000);
            //// Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var assessment = JsonConvert.DeserializeObject<QuestionModel>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(assessment, "Result is null");
            Assert.IsInstanceOfType(assessment, typeof(QuestionModel), "Wrong Model");
            Assert.AreEqual(1000, assessment.Id, "Got wrong number of Questions");
        }

        [TestMethod]
        public void Post_Question_Action_Returns_CreatedStatusCode()
        {
            // Arrange   
            var fakeQuestionModel = new QuestionDetailsModel() { Id = 1000, Description = "test fake assessment", Category = "C#" };
            questionRepository.Setup(x => x.Insert(It.IsAny<Question>()));
            questionManager = new QuestionManager(fakeUoW.Object, questionRepository.Object);
            var controller = SetupControllerContext(HttpMethod.Post, "http://localhost/api/questions/");
            //// Act
            var response = controller.Post(fakeQuestionModel);

            //// Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newQuestion = JsonConvert.DeserializeObject<QuestionModel>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(newQuestion, "Result is null");
            Assert.AreEqual(string.Format("http://localhost/api/questions/{0}", newQuestion.Id), response.Headers.Location.ToString());
        }

        [TestMethod]
        public void Post_Question_Action_Returns_BusinessException()
        {
            // Arrange   
            var fakeQuestionModel = new QuestionDetailsModel() { Id = 1000, Description = "test fake assessment" };
            questionRepository.Setup(x => x.Insert(It.IsAny<Question>()));
            questionManager = new QuestionManager(fakeUoW.Object, questionRepository.Object);
            var controller = SetupControllerContext(HttpMethod.Post, "http://localhost/api/questions/");
            
            //// Acts
            //ExceptionAssert
            ExceptionAssert.Throws<BusinessException>(() => { controller.Post(fakeQuestionModel); }, "BusinessException was not thrown.");
            //// Assert
        }

        [TestMethod]
        public void Put_Question_Returns_OKStatusCode()
        {
            // Arrange  
            Question fakeQuestion = new Question() { Id = 1000, Description = "test fake question" };

            var fakeQuestionModel = new QuestionDetailsModel() { Id = 1000, Description = "test fake question", Category = "c# 5" };
            questionRepository.Setup(x => x.Update(It.IsAny<Question>()));
            questionRepository.Setup(x => x.GetByID(1000)).Returns(fakeQuestion);
            questionManager = new QuestionManager(fakeUoW.Object, questionRepository.Object);
            var controller = SetupControllerContext(HttpMethod.Put, string.Format("http://localhost/api/questions/{0}", fakeQuestionModel.Id));

            // Act
            var response = controller.Put(fakeQuestionModel.Id, fakeQuestionModel);
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Put_Question_Returns_OKNotFoundCode()
        {
            // Arrange  
            Question fakeQuestion = new Question() { Id = 1001, Description = "test fake question" };

            var fakeQuestionModel = new QuestionDetailsModel() { Id = 1000, Description = "test fake question", Category = "C# 5" };
            questionRepository.Setup(x => x.Update(It.IsAny<Question>()));
            questionRepository.Setup(x => x.GetByID(1001)).Returns(fakeQuestion);
            questionManager = new QuestionManager(fakeUoW.Object, questionRepository.Object);
            var controller = SetupControllerContext(HttpMethod.Put, string.Format("http://localhost/api/questions/{0}", fakeQuestionModel.Id));

            // Act
            var response = controller.Put(fakeQuestionModel.Id, fakeQuestionModel);
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void Delete_Question_Returns_NoContentStatusCode()
        {
            // Arrange         
            var fakeUoW = new Mock<IUnitOfWork>();
            fakeUoW.Setup(u => u.Commit());
            var fakeQuestionModel = new QuestionModel() { Id = 1000, Description = "test fake question" };
            questionRepository.Setup(x => x.Delete(It.IsAny<Question>()));
            questionManager = new QuestionManager(fakeUoW.Object, questionRepository.Object);
            var controller = SetupControllerContext(HttpMethod.Delete, string.Format("http://localhost/api/questions/{0}", fakeQuestionModel.Id));

            // Act          
            var response = controller.Delete(1000);
            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        }
        

        private void InitQuestionCategoriesNew(Mock<IQuestionRepository> assessmentMock,
            Expression<Func<Question, bool>> filter = null,
            Func<IQueryable<Question>, IOrderedQueryable<Question>> orderBy = null,
            List<Expression<Func<Question, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<Question> fakeQuestions = GetQuestions();
            questionRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Question, bool>>>()
                , It.IsAny<Func<IQueryable<Question>, IOrderedQueryable<Question>>>()
                , It.IsAny<List<Expression<Func<Question, object>>>>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(fakeQuestions);
        }

        private static IQueryable<Question> GetQuestions()
        {
            IQueryable<Question> fakeQuestions = new List<Question> {
                new Question { Id=1, Category = "C", Description="Quesiton 1Desc"},
                new Question { Id=2, Category = "C", Description="Quesiton 2Desc"},
                new Question { Id=3, Category = "C", Description="Quesiton 3Desc"}, 
                new Question { Id=4, Category = "C", Description="Quesiton 4 Desc"}
                }.AsQueryable();
            return fakeQuestions;
        }
    }
}
