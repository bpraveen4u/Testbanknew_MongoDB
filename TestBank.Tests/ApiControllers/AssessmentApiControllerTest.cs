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
            InitAssessmentCategories(assessmentRepository, page:1, pageSize:3);
            assessmentManager = new AssessmentManager(null, assessmentRepository.Object, null);
            
            //// Act
            var pagedAssessments = assessmentManager.GetAll(page:1, pageSize:3);
            //// Assert
            Assert.IsNotNull(pagedAssessments, "Result is null");
            Assert.IsInstanceOfType(pagedAssessments, typeof(PagedEntity<Assessment>), "Wrong Model");
            Assert.AreEqual(4, pagedAssessments.PagedData.Count, "Got wrong number of Categories");
        }

        public void InitAssessmentCategories(Mock<IAssessmentRepository> assessmentMock, 
            Expression<Func<Assessment, bool>> filter = null,
            Func<IQueryable<Assessment>, IOrderedQueryable<Assessment>> orderBy = null,
            List<Expression<Func<Assessment, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<Assessment> fakeAssessments = GetAssessments();
            if (filter == null && orderBy == null && includeProperties == null)
            {
                assessmentMock.Setup(mock => mock.Get(filter, orderBy, includeProperties, page, pageSize)).Returns(fakeAssessments);
                //assessmentMock.Setup(mock => mock.Get(filter, orderBy, includeProperties, page, pageSize).Count()).Returns(4);
            }
            else
            {
                
            }
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
