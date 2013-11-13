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

namespace TestBank.Tests.ApiControllers
{
    [TestClass]
    public class AssessmentApiControllerTest
    {
        private Mock<IAssessmentRepository> assessmentRepository;
        private AssessmentManager assessmentManager;
        [TestInitialize]
        public void Setup()
        {
            assessmentRepository = new Mock<IAssessmentRepository>();
        }

        [TestMethod]
        public void Get_All_Returns_AllAssessments()
        {
            // Arrange   
            InitAssessmentCategoriesNew(assessmentRepository);
            assessmentManager = new AssessmentManager(null, assessmentRepository.Object, null);
            
            //// Act
            var pagedAssessments = assessmentManager.GetAll(page:1, pageSize:3);
            //// Assert
            Assert.IsNotNull(pagedAssessments, "Result is null");
            Assert.IsInstanceOfType(pagedAssessments, typeof(PagedEntity<Assessment>), "Wrong Model");
            Assert.AreEqual(4, pagedAssessments.TotalRecords, "Wrong number of record count");
            Assert.AreEqual(4, pagedAssessments.PagedData.Count, "Got wrong number of Assessments");
        }

        [TestMethod]
        public void Get_All_Returns_AllAssessments_Action()
        {
            // Arrange   
            IQueryable<Assessment> fakeAssessment = GetAssessments();
            assessmentRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Assessment, bool>>>()
                ,It.IsAny<Func<IQueryable<Assessment>, IOrderedQueryable<Assessment>>>()
                , It.IsAny<List<Expression<Func<Assessment, object>>>>(),It.IsAny<int?>(), It.IsAny<int?>())).Returns(fakeAssessment);
            assessmentManager = new AssessmentManager(null, assessmentRepository.Object, null);
            var controller = SetupControllerContext(HttpMethod.Get, "http://localhost/api/assessments/");

            //// Act
            var pagedAssessments = controller.GetAll();
            //// Assert
            Assert.IsNotNull(pagedAssessments, "Result is null");
            Assert.IsInstanceOfType(pagedAssessments, typeof(PagedModel<AssessmentModel>), "Wrong Model");
            Assert.AreEqual(4, pagedAssessments.TotalRecords, "Wrong number of record count");
            Assert.AreEqual(4, pagedAssessments.PagedData.Count, "Got wrong number of Assessments");
        }

        private AssessmentsController SetupControllerContext(HttpMethod method, string url)
        {
            Mapper.CreateMap<Assessment, AssessmentModel>();
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["Assessments"],
                new HttpRouteValueDictionary { { "controller", "Assessments" } });
            var controller = new AssessmentsController(assessmentManager)
            {
                Request = new HttpRequestMessage(method, url)
                {
                    Properties = 
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData } 
                }
                }
            };
            return controller;
        }

        [TestMethod]
        public void Get_One_Assessment()
        {
            // Arrange   
            Assessment fakeAssessment = new Assessment() { Id=1000, Name = "test fake assessment"};
            assessmentRepository.Setup(x => x.GetByID(1000)).Returns(fakeAssessment);

            assessmentManager = new AssessmentManager(null, assessmentRepository.Object, null);

            //// Act
            var assessment = assessmentManager.Get(1000);
            //// Assert
            Assert.IsNotNull(assessment, "Result is null");
            Assert.IsInstanceOfType(assessment, typeof(Assessment), "Wrong Model");
            Assert.AreEqual(1000, assessment.Id, "Got wrong number of Assessments");
        }

        [TestMethod]
        public void Get_One_Assessment_Action()
        {
            // Arrange   
            Assessment fakeAssessment = new Assessment() { Id = 1000, Name = "test fake assessment" };
            assessmentRepository.Setup(x => x.GetByID(1000)).Returns(fakeAssessment);

            assessmentManager = new AssessmentManager(null, assessmentRepository.Object, null);
            var controller = SetupControllerContext(HttpMethod.Get, "http://localhost/api/assessments/1000");
            //// Act
            var response = controller.Get(1000);
            //// Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var assessment = JsonConvert.DeserializeObject<AssessmentModel>(response.Content.ReadAsStringAsync().Result);
            Assert.IsNotNull(assessment, "Result is null");
            Assert.IsInstanceOfType(assessment, typeof(AssessmentModel), "Wrong Model");
            Assert.AreEqual(1000, assessment.Id, "Got wrong number of Assessments");
        }

        //public void InitAssessmentCategories(Mock<IAssessmentRepository> assessmentMock, 
        //    Expression<Func<Assessment, bool>> filter = null,
        //    Func<IQueryable<Assessment>, IOrderedQueryable<Assessment>> orderBy = null,
        //    List<Expression<Func<Assessment, object>>> includeProperties = null,
        //    int? page = null,
        //    int? pageSize = null)
        //{
        //    IQueryable<Assessment> fakeAssessments = GetAssessments();
        //    if (filter == null && orderBy == null && includeProperties == null)
        //    {
        //        assessmentMock.Setup(mock => mock.Get(filter, orderBy, includeProperties, page, pageSize)).Returns(fakeAssessments);
        //        //assessmentMock.Setup(mock => mock.Get(filter, orderBy, includeProperties, page, pageSize).Count()).Returns(4);
        //    }
        //    else
        //    {
                
        //    }
        //}

        private void InitAssessmentCategoriesNew(Mock<IAssessmentRepository> assessmentMock,
            Expression<Func<Assessment, bool>> filter = null,
            Func<IQueryable<Assessment>, IOrderedQueryable<Assessment>> orderBy = null,
            List<Expression<Func<Assessment, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<Assessment> fakeAssessments = GetAssessments();
            assessmentRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Assessment, bool>>>()
                , It.IsAny<Func<IQueryable<Assessment>, IOrderedQueryable<Assessment>>>()
                , It.IsAny<List<Expression<Func<Assessment, object>>>>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(fakeAssessments);
        }

        private static IQueryable<Assessment> GetAssessments()
        {
            IQueryable<Assessment> fakeAssessments = new List<Assessment> {
                new Assessment {Id=1, Name = "Test1", Description="Test1Desc", Duration=10},
                new Assessment {Id=2, Name = "Test2", Description="Test2Desc",Duration=20},
                new Assessment { Id=3, Name = "Test3", Description="Test3Desc",Duration=30}, 
                new Assessment { Id=4, Name = "Test3", Description="Test3Desc",Duration=30}
                }.AsQueryable();
            return fakeAssessments;
        }
    }
}
