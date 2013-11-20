using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestBank.Data;
using TestBank.Entity;
using AutoMapper;
using TestBank.API.WebHost.Models;
using System.Web.Http.Routing;
using TestBank.Business.Manager;

namespace TestBank.API.WebHost.Controllers
{
    public class AssessmentsController : BaseApiController
    {
        private readonly AssessmentManager manager;
        const int PAGE_SIZE = 2;
        public AssessmentsController(AssessmentManager manager)
        {
            this.manager = manager;
        }

        // GET api/assessments
        public PagedModel<AssessmentModel> GetAll(int page = 1)
        {
            if (page < 1) page = 1;

            var pagedAssessments = manager.GetAll(page: page, pageSize: PAGE_SIZE);

            var helper = new UrlHelper(Request);

            var links = new List<LinkModel>();
            if (page > 1 && (page - 1) < pagedAssessments.TotalPages)
            {
                links.Add(TheModelFactory.CreateLink(helper.Link("Assessments", new {page = page - 1}), "prevPage"));
            }

            if (page < pagedAssessments.TotalPages)
            {
                links.Add(TheModelFactory.CreateLink(helper.Link("Assessments", new { page = page + 1 }), "nextPage"));
            }
            
            return new PagedModel<AssessmentModel>() {
                TotalRecords = pagedAssessments.TotalRecords,
                TotalPages = pagedAssessments.TotalPages,
                Links = links,
                PagedData = pagedAssessments.PagedData.Select(a => TheModelFactory.Create(a)).ToList()
            };
        }

        // GET api/assessments/5
        public HttpResponseMessage Get(int id)
        {
            var model = TheModelFactory.Create(manager.Get(id));
            if (model == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        // POST api/assessments
        public HttpResponseMessage Post([FromBody]AssessmentModel model)
        {
            var assessment = Mapper.Map<Assessment>(model);
            assessment = manager.Insert(assessment);
            
            var newModel = TheModelFactory.Create(assessment);
            var response = Request.CreateResponse(HttpStatusCode.Created, newModel);
            var link = newModel.Links.Where(l => l.Rel == "self").FirstOrDefault();
            if (link != null)
            {
                response.Headers.Location = new Uri(link.Href);
            }

            return response;
        }

        // PUT api/assessments/5
        [HttpPut]
        [HttpPatch]
        public HttpResponseMessage Put(int id, [FromBody]AssessmentModel model)
        {
            model.Id = id;
            var assessment = Mapper.Map<Assessment>(model);
            assessment = manager.Update(assessment);

            return Request.CreateResponse(HttpStatusCode.OK, assessment);
        }

        // DELETE api/assessments/5
        public HttpResponseMessage Delete(int id)
        {
            manager.Delete(id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
