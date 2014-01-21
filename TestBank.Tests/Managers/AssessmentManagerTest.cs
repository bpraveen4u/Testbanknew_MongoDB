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

namespace TestBank.Tests.Managers
{
    [TestClass]
    public class AssessmentManagerTest
    {
        private Mock<IAssessmentRepository> assessmentRepository;
        private AssessmentManager assessmentManager;
        private Mock<IUnitOfWork> fakeUoW;
        [TestInitialize]
        public void Setup()
        {
            assessmentRepository = new Mock<IAssessmentRepository>();
            fakeUoW = new Mock<IUnitOfWork>();
            fakeUoW.Setup(u => u.Commit());
        }

        [TestMethod]
        public void GetAll_AssessmentManager_Returns_PagedAssessments()
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
        public void Get_AssessmentManager_Returns_Assessment()
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
        public void Insert_AssessmentManager_Returns_Assessment()
        {
            // Arrange   
            var fakeAssessment = new Assessment() { Id = 1000, Name = "test fake assessment", Duration = 10 };
            assessmentRepository.Setup(x => x.Insert(It.IsAny<Assessment>()));
            assessmentManager = new AssessmentManager(fakeUoW.Object, assessmentRepository.Object, null);
            //// Act
            var newAssessment = assessmentManager.Insert(fakeAssessment);

            //// Assert
            Assert.IsNotNull(newAssessment, "Result is null");
            Assert.IsInstanceOfType(newAssessment, typeof(Assessment), "Invalid Enitity");
            Assert.AreEqual(1000, newAssessment.Id);
        }

        [TestMethod]
        public void Update_AssessmentManager_Returns_Assessment()
        {
            // Arrange   
            var fakeAssessment = new Assessment() { Id = 1000, Name = "test fake assessment", Duration = 10 };
            assessmentRepository.Setup(x => x.Update(It.IsAny<Assessment>()));
            assessmentManager = new AssessmentManager(fakeUoW.Object, assessmentRepository.Object, null);
            //// Act
            var newAssessment = assessmentManager.Update(fakeAssessment);

            //// Assert
            Assert.IsNotNull(newAssessment, "Result is null");
            Assert.IsInstanceOfType(newAssessment, typeof(Assessment), "Invalid Enitity");
            Assert.AreEqual(1000, newAssessment.Id);
        }

        [TestMethod]
        public void Delete_AssessmentManager_Returns_void()
        {
            // Arrange   
            var fakeAssessment = new Assessment() { Id = 1000, Name = "test fake assessment", Duration = 10 };
            assessmentRepository.Setup(x => x.Delete(1000));
            assessmentManager = new AssessmentManager(fakeUoW.Object, assessmentRepository.Object, null);
            //// Act
            assessmentManager.Delete(1000);

            //// Assert
            //Assert.IsNotNull(newAssessment, "Result is null");
            //Assert.IsInstanceOfType(newAssessment, typeof(Assessment), "Invalid Enitity");
            //Assert.AreEqual(1000, newAssessment.Id);
        }
        
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
                new Assessment { Id=1, Name = "Test1", Description="Test1Desc", Duration=10},
                new Assessment { Id=2, Name = "Test2", Description="Test2Desc", Duration=20},
                new Assessment { Id=3, Name = "Test3", Description="Test3Desc", Duration=30}, 
                new Assessment { Id=4, Name = "Test3", Description="Test3Desc", Duration=30}
                }.AsQueryable();
            return fakeAssessments;
        }
    }
}
