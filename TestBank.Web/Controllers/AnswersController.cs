#region using..
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestBank.Entity.Models;
using TestBank.Web.Models;
using TestBank.Web.ViewModels;
using TestBank.Web.Infrastructure.ServiceProxy;
using TestBank.Web.Infrastructure.AutoMapper;
using AutoMapper;
using PagedList;
//using TestBank.Entity.Utilities;
using NLog;
using TestBank.Web.Infrastructure.Attributes;
using TestBank.Web.Infrastructure.Utilities;
using System.IO;
using TestBank.Web.Filters;
using TestBank.Web.Infrastructure.Encryption;
using TestBank.Entity.Errors;
#endregion

namespace TestBank.Web.Controllers
{
    public class AnswersController : BaseController
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private const string clienSecret = "&*@Pc43fass%wTr5!4ord";

        public ViewResult Results(int testId)
        {
            var urlParams = new List<Tuple<string, string>>();
            urlParams.Add(new Tuple<string, string>("testId", testId.ToString()));

            var listAnswers = TestBankApiProxy.Get<List<UserAnswerModel>>(ApiKey,ResourceEndPoint.UserAnswers_All, urlParams);
            IEnumerable<UserAnswersResultViewModel> model = null;
            if (listAnswers != null)
            {
                model = Mapper.Map<IEnumerable<UserAnswerModel>, IEnumerable<UserAnswersResultViewModel>>(listAnswers);
            }
            return View(model);
        }

        [AllowAnonymous]
        //[CryptoValueProvider]
        public ActionResult Start(int testId)
        {
            if (testId == 0)
            {
                throw new Exception("invalid test id.");
            }

            if (Request.IsAuthenticated)
            {
                SetUserIdentity(User.Identity.Name);
                return AddAnswersAndRedirect(testId);
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        //[CryptoValueProvider]
        public ActionResult Start(int testId, UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                if (testId > 0 && userViewModel != null)
                {
                    var user = Mapper.Map<UserViewModel, UserModel>(userViewModel);
                    user.Role = Roles.Student;
                    try
                    {
                        user = TestBankApiProxy.Post<UserModel>(ApiKey, user, ResourceEndPoint.User_Post);
                    }
                    catch (ApiError error)
                    {
                        if (error.Errors != null)
                        {
                            foreach (var item in error.Errors)
                            {
                                ModelState.AddModelError("", item);
                            }
                        }
                        else
                            ModelState.AddModelError("", error.Message);
                        
                        return View();
                    }
                    
                    if (user !=  null)
                    {
                        if (!Request.IsAuthenticated)
                        {
                            ApiKey = TestBankApiProxy.Authenticate(new Credentials() { User = user.Id, Password = "password" });
                            SetUserIdentity(user.Id);
                            return AddAnswersAndRedirect(testId);
                        }
                    }
                }
            }
            return View();
        }

        [AllowAnonymous]
        //[CryptoValueProvider]
        public ActionResult Save(int testId, int? responseId, int? page)
        {
            if (testId == 0)
            {
                throw new Exception("invalid test id.");
            }
            if (responseId.HasValue && responseId.Value == 0)
            {
                throw new Exception("invalid response Id.");
            }
            if (Session["secret"] == null)
            {
                return RedirectToAction("Start", new { testId = EncryptDecryptQueryString.Encrypt(testId.ToString()) });
            }
            return GetAnswerViewModel(testId, responseId, page);
        }

        [AllowAnonymous] 
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Save")]
        public ActionResult Save(UserAnswersInputViewModel ansInputModel)
        {
            ClearModelState();
            if (!(ValidateUserIdentity(ansInputModel.UserId)))
            {
                return View("Unauthorized");
            }
            
            var answers = SaveAnswer(ansInputModel);
            return GetAnswerViewModel(ansInputModel.AssessmentId, ansInputModel.Id, ansInputModel.CurrentPageId);
        }

        [AllowAnonymous]
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Save & Next")]
        public ActionResult SaveNext(UserAnswersInputViewModel ansInputModel)
        {
            ClearModelState();
            if (!(ValidateUserIdentity(ansInputModel.UserId)))
            {
                return View("Unauthorized");
            }
            var answers = SaveAnswer(ansInputModel);
            
            if (answers != null)
            {
                return GetAnswerViewModel(ansInputModel.AssessmentId, ansInputModel.Id, ansInputModel.CurrentPageId + 1);
            }
            else
            {
                return GetAnswerViewModel(ansInputModel.AssessmentId, ansInputModel.Id, ansInputModel.CurrentPageId);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Finish")] 
        public ActionResult Finish(UserAnswersInputViewModel ansInputModel)
        {
            if (!(ValidateUserIdentity(ansInputModel.UserId)))
            {
                return View("Unauthorized");
            }
            ansInputModel.IsTestCompleted = true;
            var answers = SaveAnswer(ansInputModel);
            if (answers == null)
            {
                ClearModelState();
                return GetAnswerViewModel(ansInputModel.AssessmentId, ansInputModel.Id, ansInputModel.CurrentPageId);
            }
            
            return RedirectToAction("Finish", new { testId = EncryptDecryptQueryString.Encrypt(ansInputModel.AssessmentId.ToString()) });
        }

        [AllowAnonymous]
        [HttpGet]
        //[CryptoValueProvider]
        public ActionResult Finish(int testId)
        {
            return View("Finish");
        }


        public ActionResult Export(int testId, string output)
        {
            //api/UserAnswers/Export?testId=9&output=Excel
            var urlParams = new List<Tuple<string, string>>();
            urlParams.Add(new Tuple<string, string>("testId", testId.ToString()));
            urlParams.Add(new Tuple<string, string>("output", output));
            var exportOutput = TestBankApiProxy.DownloadFile(ApiKey,"api/UserAnswers/Export?testId={testId}&output={output}", urlParams);

            Stream report = new MemoryStream(exportOutput);
            return new FileStreamResult(report, "application/ms-excel")
            {
                FileDownloadName = string.Format("TestResults_{0:yyyyMMddHHmmss}.xlsx",DateTime.Now)
            };
        }

        #region Private Methods
        private ViewResult GetAnswerViewModel(int id, int? responseId, int? page)
        {
            logger.Debug("get answer details");
            int pageSize = 4;
            page = page ?? 1;
            int pageIndex = page.Value == 0 ? 1 : page.Value;

            UserAnswersInputViewModel answerModel = null;
            UserAnswerModel answer = null;
            if (responseId.HasValue && responseId.Value > 0)
            {
                logger.Debug("response id = {0}", responseId.Value);
                answer = TestBankApiProxy.Get<UserAnswerModel>(ApiKey, ResourceEndPoint.UserAnswers_Get, responseId.Value);
                logger.Debug("Answer id = {0}, testid = {1}", answer.Id, answer.AssessmentId);
                answerModel = answer.MapTo<UserAnswersInputViewModel>();

                if (!(ValidateUserIdentity(answerModel.UserId)))
                {
                    return View("Unauthorized");
                }
            }
            else
            {
                //if (!(ValidateUserIdentity(answerModel.UserId)))
                //{
                //    return View("Unauthorized");
                //}
            }
            var assessment = GetAssessmentDetails(id, pageIndex, pageSize);

            if (answerModel != null && assessment != null && answer != null)
            {
                int i = (pageIndex - 1) * pageSize;
                var mappedQuestions = (from q in assessment.QuestionDetails
                                       select new UserAnswersInputViewModel.AnswerInputViewModel()
                                       {
                                           QuestionId = Convert.ToInt32(q.Id),
                                           Description = q.Description,
                                           Nr = ++i,
                                           OptionItems = q.Options.Select(x => new SelectListItem
                                           {
                                               Text = x.Text,
                                               Value = x.Id,
                                               Selected = (x.Id == answer.Answers.Where(o => o.Question.QuestionId == q.Id).DefaultIfEmpty(new UserAnswerModel.Answer()).FirstOrDefault().SelectedOption)
                                           }
                                               ).ToList(),
                                       }).ToList();

                answerModel.QuestionAnswers = mappedQuestions;
                answerModel.PagingMetaData = new StaticPagedList<UserAnswersInputViewModel.AnswerInputViewModel>(mappedQuestions, pageIndex, pageSize, assessment.TotalQuestions).GetMetaData();
                answerModel.CurrentPageId = answerModel.PagingMetaData.PageNumber;
                answerModel.AssessmentId = id;
                answerModel.Duration = assessment.Duration;
                if (answerModel.StartDateTime.HasValue)
                {
                    answerModel.JsTime = answerModel.StartDateTime.Value.ToJavascriptTimestamp();
                }
               
            }

            return View(answerModel);
        }

        private UserAnswerModel SaveAnswer(UserAnswersInputViewModel ansInputModel)
        {
            
            var toAnswerEntity = Mapper.Map<UserAnswersInputViewModel, UserAnswerModel>(ansInputModel);
            UserAnswerModel answers = null;
            try
            {
                answers = TestBankApiProxy.Put<UserAnswerModel>(ApiKey, toAnswerEntity, ResourceEndPoint.UserAnswers_Put, ansInputModel.Id);
            }
            catch (ApiError error)
            {
                foreach (var item in error.Errors)
                {
                    ModelState.AddModelError("", item);
                }
            }
            return answers;
        }
        
        private AssessmentModel GetAssessmentDetails(int assessmentId, int pageIndex, int pageSize)
        {
            var urlParams = new List<Tuple<string, string>>();
            urlParams.Add(new Tuple<string, string>("id", assessmentId.ToString()));
            urlParams.Add(new Tuple<string, string>("pageIndex", pageIndex.ToString()));
            urlParams.Add(new Tuple<string, string>("pageSize", pageSize.ToString()));
            var assessment = TestBankApiProxy.Get<AssessmentModel>(ApiKey, "api/Assessments/{id}?pageIndex={pageIndex}&pageSize={pageSize}", urlParams);
            return assessment;
        }

        private AssessmentViewModel GetAssessment(int id)
        {
            var assessment = TestBankApiProxy.Get<AssessmentModel>(ApiKey, ResourceEndPoint.Assessments_Get, id);
            AssessmentViewModel assessmentviewmodel = null;
            if (assessment != null)
            {
                assessmentviewmodel = Mapper.Map<AssessmentModel, AssessmentViewModel>(assessment);
            }
            return assessmentviewmodel;
        }

        private void ClearModelState()
        {
            ModelState.Remove("CurrentPageId");
            var keys = ModelState.Keys.ToArray();
            foreach (var k in keys)
            {
                if (k.StartsWith("QuestionAnswers["))
                {
                    ModelState.Remove(k);
                }
            }
        }

        private void SetUserIdentity(string userId)
        {
            Session["secret"] = new Credentials() { User = userId, Password = userId + clienSecret };
        }

        private bool ValidateUserIdentity(string userId)
        {
            var user = TestBankApiProxy.Get<UserModel>(ApiKey, ResourceEndPoint.User_Get, userId);

            if (user != null && Session["secret"] != null && Session["secret"] is Credentials)
            {
                var credentials = Session["secret"] as Credentials;
                if (!(string.IsNullOrWhiteSpace(credentials.User) && string.IsNullOrWhiteSpace(credentials.Password)))
                {
                    if (credentials.User == user.Id && credentials.Password == user.Id + clienSecret)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private ActionResult AddAnswersAndRedirect(int testId)
        {
            //SetUserIdentity(User.Identity.Name);
            var ansInputModel = new UserAnswersInputViewModel() { Id = 0, AssessmentId = testId, QuestionAnswers = new List<UserAnswersInputViewModel.AnswerInputViewModel>() };
            var toAnswerEntity = Mapper.Map<UserAnswersInputViewModel, UserAnswerModel>(ansInputModel);

            try
            {
                var answers = TestBankApiProxy.Post<UserAnswerModel>(ApiKey, toAnswerEntity, ResourceEndPoint.UserAnswers_Post);

                var responseId = Convert.ToInt32(answers.Id);
                return RedirectToAction("Save", new { testId = EncryptDecryptQueryString.Encrypt(testId.ToString()), responseId = EncryptDecryptQueryString.Encrypt(responseId.ToString()), page = 1 });
            }
            catch (ApiError error)
            {
                foreach (var item in error.Errors)
                {
                    ModelState.AddModelError("", item);
                }
            }

            return View();
        }

        #endregion

    }
}