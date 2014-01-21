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
    public class QuestionsController : BaseApiController
    {
        private readonly QuestionManager manager;
        const int PAGE_SIZE = 3;
        public QuestionsController(QuestionManager manager)
        {
            this.manager = manager;
        }

        // GET api/questions
        public PagedModel<QuestionModel> GetAll(int page = 1)
        {
            if (page < 1) page = 1;

            var pagedQuestions = manager.GetAll(page: page, pageSize: PAGE_SIZE);

            var helper = new UrlHelper(Request);

            var links = new List<LinkModel>();
            if (page > 1 && (page - 1) < pagedQuestions.TotalPages)
            {
                links.Add(TheModelFactory.CreateLink(helper.Link("Questions", new {page = page - 1}), "prevPage"));
            }

            if (page < pagedQuestions.TotalPages)
            {
                links.Add(TheModelFactory.CreateLink(helper.Link("Questions", new { page = page + 1 }), "nextPage"));
            }
            
            return new PagedModel<QuestionModel>() {
                TotalRecords = pagedQuestions.TotalRecords,
                TotalPages = pagedQuestions.TotalPages,
                Links = links,
                PagedData = pagedQuestions.PagedData.Select(a => TheModelFactory.Create(a)).ToList()
            };
        }

        // GET api/questions/5
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var question = manager.Get(id);
            
            if (question == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var model = TheModelFactory.CreateDetails(question);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        // GET api/questions/5/options
        [HttpGet]
        public HttpResponseMessage GetQuestionOptions(int questionId, string optionId = null)
        {
            var question = manager.Get(questionId);
            
            if (question == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var model = TheModelFactory.CreateDetails(question);

            if (optionId == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, model.Options);
            }
            else
            {
                if (model.Options != null)
                {
                    var option = model.Options.Where(o => o.Id == optionId).SingleOrDefault();
                    if (option != null)
                        return Request.CreateResponse(HttpStatusCode.OK, model.Options.Where(o => o.Id == optionId).SingleOrDefault());
                }
            }

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST api/questions
        public HttpResponseMessage Post([FromBody]QuestionDetailsModel model)
        {
            Mapper.AssertConfigurationIsValid();
            var questions = Mapper.Map<Question>(model);
            questions = manager.Insert(questions);
            
            var newModel = TheModelFactory.CreateDetails(questions);
            var response = Request.CreateResponse(HttpStatusCode.Created, newModel);
            var link = newModel.Links.Where(l => l.Rel == "self").FirstOrDefault();
            if (link != null)
            {
                response.Headers.Location = new Uri(link.Href);
            }

            return response;
        }

        // PUT api/questions/5
        [HttpPut]
        [HttpPatch]
        public HttpResponseMessage Put(int id, [FromBody]QuestionDetailsModel model)
        {
            model.Id = id;
            var originalQuestion = manager.Get(id);
            if (originalQuestion == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            Mapper.AssertConfigurationIsValid();
            var question = Mapper.Map<Question>(model);
            question.CreatedDate = originalQuestion.CreatedDate;
            question.CreatedUser = originalQuestion.CreatedUser;
            question = manager.Update(question);
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
