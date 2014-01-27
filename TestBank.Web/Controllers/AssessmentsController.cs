using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestBank.Entity.Models;
using TestBank.Web.Models;
using TestBank.Web.Infrastructure.ServiceProxy;
using TestBank.Web.ViewModels;
using AutoMapper;
using TestBank.Web.Infrastructure.AutoMapper.Profiles.Resolvers;
//using TestBank.Entity.Utilities;
using System.Configuration;
using PagedList;

namespace TestBank.Web.Controllers
{   
    public class AssessmentsController : BaseController
    {
        private int pageSize = 10;

        //
        // GET: /Assessments/

        public ViewResult Index(int? page)
        {
            //var listAssessments = TestBankApiProxy.Get<List<AssessmentModel>>(ApiKey, ResourceEndPoint.Assessments_All);
            //IEnumerable<AssessmentViewModel> model = null;
            //if (listAssessments != null)
            //{
            //    model = Mapper.Map<IEnumerable<AssessmentModel>, IEnumerable<AssessmentViewModel>>(listAssessments);
            //}
            //return View(model);

            int pageIndex = page ?? 1;
            var urlParams = new List<Tuple<string, string>>();
            urlParams.Add(new Tuple<string, string>("pageIndex", pageIndex.ToString()));
            //urlParams.Add(new Tuple<string, string>("pageSize", pageSize.ToString()));
            var pagedResults = TestBankApiProxy.Get<PagedModel<AssessmentModel>>(ApiKey, "/Assessments?page={pageIndex}", urlParams);
            if (pagedResults != null && pagedResults.PagedData != null)
            {
                //Mapper.AssertConfigurationIsValid();
                var model = Mapper.Map<IEnumerable<AssessmentModel>, IEnumerable<AssessmentViewModel>>(pagedResults.PagedData);
                return View(new StaticPagedList<AssessmentViewModel>(model, pageIndex, pageSize, pagedResults.TotalRecords));
            }
            return null;
        }

        //
        // GET: /Assessments/Details/5

        public ViewResult Details(int id)
        {
            var assessment = TestBankApiProxy.Get<AssessmentDetailsModel>(ApiKey, ResourceEndPoint.Assessments_Get, id);
            AssessmentViewModel assessmentViewModel = null;
            if (assessment != null)
            {
                assessmentViewModel = Mapper.Map<AssessmentDetailsModel, AssessmentViewModel>(assessment);
            }
            return View(assessmentViewModel);
        }

        //
        // GET: /Assessments/Create

        public ActionResult Create()
        {
            var assessmentviewmodel = new AssessmentViewModel();
            //GetAssessmentData(assessmentviewmodel);
            assessmentviewmodel.Questions = new List<QuestionReferenceViewModel>();
            return View(assessmentviewmodel);
        }

        private void GetAssessmentData(AssessmentViewModel assessmentviewmodel)
        {
            //assessmentviewmodel.AllQuestions = new List<QuestionReferenceViewModel>();
            //var allQuestions = GetAllQuestions();
            //assessmentviewmodel.AllQuestions.AddRange(allQuestions);
            //assessmentviewmodel.AllQuestionIds = allQuestions.Select(q => q.QuestionId).ToArray();
        }

        //
        // POST: /Assessments/Create

        [HttpPost]
        public ActionResult Create(AssessmentViewModel assessmentViewModel)
        {
            if (ModelState.IsValid)
            {
                if (assessmentViewModel != null)
                {
                    assessmentViewModel.Link = string.Format("{0}Answers/Start?testId={{id}}", ConfigurationManager.AppSettings.Get("AppHostUrl"));
                    assessmentViewModel.Questions = assessmentViewModel.QuestionIds.Select(x => new QuestionReferenceViewModel { QuestionId = x }).ToList();
                    var assessment = Mapper.Map<AssessmentViewModel, AssessmentDetailsModel>(assessmentViewModel);
                    assessment = TestBankApiProxy.Post<AssessmentDetailsModel>(ApiKey, assessment, ResourceEndPoint.Assessments_Post);
                }

                return RedirectToAction("Index");
            }

            GetAssessmentData(assessmentViewModel);
            return View(assessmentViewModel);
        }

        //
        // GET: /Assessments/Edit/5

        public ActionResult Edit(int id)
        {
            AssessmentViewModel assessmentviewmodel = GetAssessment(id);

            //assessmentviewmodel.AllQuestions = new List<QuestionReferenceViewModel>();
            GetAssessmentData(assessmentviewmodel);
            if (assessmentviewmodel.Questions != null && assessmentviewmodel.Questions.Count >0)
            {
                assessmentviewmodel.QuestionIds = assessmentviewmodel.Questions.Select(q => q.QuestionId).ToArray();
            }
            else
            {
                assessmentviewmodel.QuestionIds = new int [0];
            }
            

            return View(assessmentviewmodel);
        }

        private List<QuestionReferenceViewModel> GetAllQuestions()
        {
            //var urlParams = new List<Tuple<string,string>>();
            //urlParams.Add(new Tuple<string, string>("countOnly", "false"));
            var listQuestions = TestBankApiProxy.Get<List<QuestionModel>>(ApiKey, ResourceEndPoint.Question_All);
            var allQuestions = listQuestions.Select(q => new QuestionReferenceViewModel() { QuestionId = Convert.ToInt32(q.Id), Description = q.Description, Sort = q.Sort }).ToList();

            return allQuestions;
        }

        //
        // POST: /Assessments/Edit/5

        [HttpPost]
        public ActionResult Edit(AssessmentViewModel assessmentViewModel)
        {
            if (ModelState.IsValid)
            {
                if (assessmentViewModel != null)
                {
                    assessmentViewModel.Questions = assessmentViewModel.QuestionIds.Select(x => new QuestionReferenceViewModel { QuestionId = x }).ToList();
                    var assessment = TestBankApiProxy.Get<AssessmentModel>(ApiKey, ResourceEndPoint.Assessments_Get, assessmentViewModel.Id);
                    var assessmentUpd = Mapper.Map<AssessmentViewModel, AssessmentModel>(assessmentViewModel);

                    assessment = TestBankApiProxy.Put<AssessmentModel>(ApiKey, assessmentUpd, ResourceEndPoint.Assessments_Put, assessmentViewModel.Id);
                    return RedirectToAction("Index");
                }
            }
            GetAssessmentData(assessmentViewModel);
            return View(assessmentViewModel);
        }

        //
        // GET: /Assessments/Delete/5
 
        public ActionResult Delete(int id)
        {
            AssessmentViewModel assessmentviewmodel = GetAssessment(id);
            return View(assessmentviewmodel);
        }

        //
        // POST: /Assessments/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var assessment = TestBankApiProxy.Delete<AssessmentModel>(ApiKey, ResourceEndPoint.Assessments_Get, id);
            return RedirectToAction("Index");
        }

        private AssessmentViewModel GetAssessment(int id)
        {
            var assessment = TestBankApiProxy.Get<AssessmentDetailsModel>(ApiKey, ResourceEndPoint.Assessments_Get, id);
            AssessmentViewModel assessmentviewmodel = null;
            if (assessment != null)
            {
                assessmentviewmodel = Mapper.Map<AssessmentDetailsModel, AssessmentViewModel>(assessment);
            }
            return assessmentviewmodel;
        }

        [HttpGet]
        public ActionResult Start(int id)
        {
            AssessmentViewModel assessmentviewmodel = GetAssessment(id);
            return View(assessmentviewmodel);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}