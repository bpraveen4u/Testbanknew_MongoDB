using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestBank.Web.ViewModels;
using TestBank.Web.Models;
using TestBank.Web.Infrastructure.ServiceProxy;
using TestBank.Entity.Models;
using AutoMapper;
using AutoMapper.Mappers;
using RestSharp;
using PagedList;
using TestBank.Entity.Errors;
using TestBank.Web.Infrastructure.Extensions;

namespace TestBank.Web.Controllers
{   
    public class QuestionsController : BaseController
    {
        private int pageSize = 10;
        //
        // GET: /Questions/

        public ViewResult Index(int? page)
        {
            int pageIndex = page ?? 1;
            var urlParams = new List<Tuple<string,string>>();
            urlParams.Add(new Tuple<string, string>("pageIndex", pageIndex.ToString()));
            //urlParams.Add(new Tuple<string, string>("pageSize", pageSize.ToString()));
            var pagedResults = TestBankApiProxy.Get<PagedModel<QuestionModel>>(ApiKey, "/Questions?page={pageIndex}", urlParams);
            if (pagedResults != null && pagedResults.PagedData != null)
            {
                //Mapper.AssertConfigurationIsValid();
                var model = Mapper.Map<IEnumerable<QuestionModel>, IEnumerable<QuestionViewModel>>(pagedResults.PagedData);
                return View(new StaticPagedList<QuestionViewModel>(model, pageIndex, pageSize, pagedResults.TotalRecords));
            }
            return null;
        }

        public ActionResult GetByCategory(string category)
        {
            var urlParams = new List<Tuple<string, string>>();
            urlParams.Add(new Tuple<string, string>("categoryName", category));
            var results = TestBankApiProxy.Get<List<QuestionModel>>(ApiKey, "/Questions/category/{categoryName}", urlParams);
            if (results != null)
            {
                var questionviewmodel = Mapper.Map<IEnumerable<QuestionModel>, IEnumerable<QuestionViewModel>>(results);
                return this.JsonEx(questionviewmodel);
            }
            return null;
        }

        //
        // GET: /Questions/Details/5

        public ViewResult Details(int id)
        {
            var question = TestBankApiProxy.Get<QuestionDetailsModel>(ApiKey, ResourceEndPoint.Question_Get, id);
            QuestionViewModel questionviewmodel = null; // context.QuestionViewModels.Single(x => x.Id == id);
            if (question != null)
            {
                questionviewmodel = Mapper.Map<QuestionDetailsModel, QuestionViewModel>(question);
            }
            return View(questionviewmodel);
        }

        //
        // GET: /Questions/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Questions/Create

        [HttpPost]
        public ActionResult Create(QuestionViewModel questionviewmodel)
        {
            //System.Diagnostics.Debugger.Launch();
            if (ModelState.IsValid)
            {
                if (questionviewmodel != null)
                {
                    var question = Mapper.Map<QuestionViewModel, QuestionDetailsModel>(questionviewmodel);
                    question = TestBankApiProxy.Post<QuestionDetailsModel>(ApiKey, question, ResourceEndPoint.Question_Post);
                }
                
                return RedirectToAction("Index");
            }

            return View(questionviewmodel);
        }
        
        //
        // GET: /Questions/Edit/5

        public ActionResult Edit(int id)
        {
            QuestionViewModel questionviewmodel = GetQuestion(id);
            
            return View(questionviewmodel);
        }

        private QuestionViewModel GetQuestion(int id)
        {
            var question = TestBankApiProxy.Get<QuestionDetailsModel>(ApiKey, ResourceEndPoint.Question_Get, id);
            QuestionViewModel questionviewmodel = null;
            if (question != null)
            {
                questionviewmodel = Mapper.Map<QuestionDetailsModel, QuestionViewModel>(question);
            }
            return questionviewmodel;
        }

        //
        // POST: /Questions/Edit/5

        [HttpPost]
        [TestBank.Web.Filters.ApiError]
        public ActionResult Edit(QuestionViewModel questionviewmodel)
        {
            if (ModelState.IsValid)
            {
                if (questionviewmodel != null)
                {
                    var question = TestBankApiProxy.Get<QuestionDetailsModel>(ApiKey, ResourceEndPoint.Question_Get, questionviewmodel.Id);
                    var questionUpd = Mapper.Map<QuestionViewModel, QuestionDetailsModel>(questionviewmodel);
                    try
                    {
                        question = TestBankApiProxy.Put<QuestionDetailsModel>(ApiKey, questionUpd, ResourceEndPoint.Question_Put, questionviewmodel.Id);
                    }
                    catch (ApiError ex)
                    {
                        foreach (var item in ex.Errors)
                        {
                            ModelState.AddModelError("", item);
                        }
                        return View(questionviewmodel);
                    }
                    
                    return RedirectToAction("Index");
                }
            }

            return View(questionviewmodel);
        }

        //
        // GET: /Questions/Delete/5

        public ActionResult Delete(int id)
        {
            QuestionViewModel questionviewmodel = GetQuestion(id);
            return View(questionviewmodel);
        }

        //
        // POST: /Questions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //var question = TestBankApiProxy.Delete<Question>(ApiKey, ResourceEndPoint.Question_Get, id);
            return RedirectToAction("Index");
        }

        public PartialViewResult AddOption()
        {
            return PartialView("_CreateOrEditOptions", new QuestionViewModel.OptionViewModel());
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    context.Dispose();
            //}
            base.Dispose(disposing);
        }
    }
}