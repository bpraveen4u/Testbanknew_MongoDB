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