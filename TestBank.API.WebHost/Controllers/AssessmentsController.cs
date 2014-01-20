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
        private readonly QuestionManager questionManager;
        const int PAGE_SIZE = 2;
        public AssessmentsController(AssessmentManager manager, QuestionManager questionManager)
        {
            this.manager = manager;
            this.questionManager = questionManager;
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
            var assessment = manager.Get(id);

            if (assessment == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var model = TheModelFactory.CreateDetails(assessment);

            if (assessment.Questions != null && assessment.Questions.Length > 0)
            {
                model.Questions = new List<QuestionModel>();
                foreach (var qId in assessment.Questions)
                {
                    var question = questionManager.Get(qId);
                    if (question != null)
                    {
                        model.Questions.Add(TheModelFactory.CreateDetails(question, id, "AssessmentQuestions"));
                    }
                }
            }
           
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        // GET api/assessments/5/questions
        public HttpResponseMessage GetAssessmentQuestions(int assessmentId)
        {
            var assessment = manager.Get(assessmentId);

            if (assessment == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var model = TheModelFactory.CreateDetails(assessment);

            var questions = new List<QuestionModel>();
            if (assessment.Questions != null && assessment.Questions.Length > 0)
            {
                foreach (var qId in assessment.Questions)
                {
                    var question = questionManager.Get(qId);
                    if (question != null)
                    {
                        questions.Add(TheModelFactory.CreateDetails(question, assessmentId, "AssessmentQuestions"));
                    }
                }

            }

            return Request.CreateResponse(HttpStatusCode.OK, questions);
        }

        // GET api/assessments/5
        public HttpResponseMessage GetAssessmentQuestions(int assessmentId, int id)
        {
            var assessment = manager.Get(assessmentId);

            if (assessment == null || assessment.Questions == null || !assessment.Questions.Contains(id))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var model = TheModelFactory.CreateDetails(assessment);

            if (assessment.Questions != null && assessment.Questions.Length > 0)
            {
                var question = questionManager.Get(id);
                if (null == question)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateDetails(question, assessmentId, "AssessmentQuestions"));
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST api/assessments
        public HttpResponseMessage Post([FromBody]AssessmentDetailsModel model)
        {
            //Mapper.AssertConfigurationIsValid();
            var assessment = Mapper.Map<Assessment>(model);
            //assessment.Questions = model.Questions.Select(q => q.Id).ToArray();
            assessment = manager.Insert(assessment);
            
            var newModel = TheModelFactory.CreateDetails(assessment);
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
        public HttpResponseMessage Put(int id, [FromBody]AssessmentDetailsModel model)
        {
            model.Id = id;
            var originalAssessment = manager.Get(id);
            if (originalAssessment == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            
            var assessment = Mapper.Map<Assessment>(model);
            assessment.CreatedDate = originalAssessment.CreatedDate;
            assessment.CreatedUser = originalAssessment.CreatedUser;
            assessment = manager.Update(assessment);
            //var model = TheModelFactory.CreateDetails(manager.Get(id));

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.CreateDetails(manager.Get(id)));
        }

        // DELETE api/assessments/5
        public HttpResponseMessage Delete(int id)
        {
            manager.Delete(id);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
