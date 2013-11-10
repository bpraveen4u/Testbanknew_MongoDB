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

namespace TestBank.API.WebHost.Controllers
{
    public class AssessmentsController : BaseApiController
    {
        const int PAGE_SIZE = 2;
        public AssessmentsController(UnitOfWork unitOfWork) : 
            base(unitOfWork)
        {
        }

        // GET api/assessments
        public IEnumerable<AssessmentModel> GetAll(int page = 1)
        {
            if (page < 1) page = 1;
            
            return UnitOfWork.AssessmentRepository.Get(page: page, pageSize: PAGE_SIZE).ToList().Select(a => TheModelFactory.Create(a));
        }

        // GET api/assessments/5
        public HttpResponseMessage Get(int id)
        {
            var model = TheModelFactory.Create(UnitOfWork.AssessmentRepository.GetByID(id));
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
            UnitOfWork.AssessmentRepository.Insert(assessment);

            UnitOfWork.Commit();

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
        public void Put(int id, [FromBody]AssessmentModel model)
        {
        }

        // DELETE api/assessments/5
        public void Delete(int id)
        {
            
        }
    }
}
