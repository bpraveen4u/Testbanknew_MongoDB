using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using TestBank.Data;
using System.Web.Http.Routing;
using TestBank.Entity;
using AutoMapper;

namespace TestBank.API.WebHost.Models
{
    public class ModelFactory
    {
        private UrlHelper urlHelper;
        //private IUnitOfWork unitOfWork;
        public ModelFactory(HttpRequestMessage request)
        {
            this.urlHelper = new UrlHelper(request);
            //this.unitOfWork = unitOfWork;
        }

        public AssessmentModel Create(Assessment assessment)
        {
            if (assessment == null)
            {
                return null;
            }
            var testModel = Mapper.Map<AssessmentModel>(assessment);
            testModel.Links = new List<LinkModel>()
                {
                    CreateLink(urlHelper.Link("Assessments", new { Id = assessment.Id }), "self")
                };
            return testModel;
        }

        public AssessmentDetailsModel CreateDetails(Assessment assessment)
        {
            if (assessment == null)
            {
                return null;
            }
            var testModel = Mapper.Map<AssessmentDetailsModel>(assessment);
            testModel.Links = new List<LinkModel>()
                {
                    CreateLink(urlHelper.Link("Assessments", new { Id = assessment.Id }), "self")
                };

            return testModel;
        }

        public QuestionModel Create(Question question)
        {
            if (question == null)
            {
                return null;
            }
            var questionModel = Mapper.Map<QuestionModel>(question);
            questionModel.Links = new List<LinkModel>()
                {
                    CreateLink(urlHelper.Link("Questions", new { Id = question.Id }), "self")
                };
            return questionModel;
        }

        public QuestionDetailsModel CreateDetails(Question question, int assessmentId = 0, string routeTemplate = null)
        {
            if (question == null)
            {
                return null;
            }
            var questionModel = Mapper.Map<QuestionDetailsModel>(question);
            if (routeTemplate == null || routeTemplate.Equals("Questions", StringComparison.InvariantCultureIgnoreCase))
            {
                questionModel.Links = new List<LinkModel>()
                {
                    CreateLink(urlHelper.Link("Questions", new { Id = question.Id }), "self")
                };
            }
            else if (routeTemplate.Equals("AssessmentQuestions", StringComparison.InvariantCultureIgnoreCase))
            {
                questionModel.Links = new List<LinkModel>()
                {
                    CreateLink(urlHelper.Link("AssessmentQuestions", new { Id = question.Id, assessmentId = assessmentId }), "self")
                };
            }
            
            return questionModel;
        }


        public LinkModel CreateLink(string href, string rel, string method = "GET", bool isTemplated = false)
        {
            return new LinkModel()
            {
                Href = href,
                Rel =rel,
                Method = method,
                IsTemplated = isTemplated
            };
        }
    }

}