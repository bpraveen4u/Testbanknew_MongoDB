using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Web.Http;
using TestBank.Business.Exceptions;
using TestBank.Business.Core.Validators;
//using OfficeOpenXml;
using TestBank.Entity.Sys;
using TestBank.Data.Repositories;
using TestBank.Entity;
using OfficeOpenXml;
using TestBank.Infrastructure.Extensions;

namespace TestBank.Business.Manager
{
    public class UserAnswersManager
    {
        private readonly IUserAnswerRepository repository;
        private readonly AssessmentManager assessmentManager;
        private readonly QuestionManager questionManager;
        //TestBank manager = new ProductManager(new ProductRepositorySqlServer());
        //public UserAnswersManager()
        //    : this(new UserAnswerMongoRepository(), new AssessmentManager(null, new AssessmentMongoRepository(), new QuestionMongoRepository()))
        //{
        //    //by default uses the raven database
        //}

        public UserAnswersManager(IUserAnswerRepository repository, AssessmentManager assessmentManager, QuestionManager questionManager)
        {
            this.repository = repository;
            this.assessmentManager = assessmentManager;
            this.questionManager = questionManager;
        }

        public IEnumerable<UserAnswer> GetAll(int assessmentId)
        {
            //string assessmentId = testId;
            //var assessment = assessmentManager.Get(assessmentId);
            var assessment = repository.GetByID(assessmentId);
            if (assessment != null && assessment.CreatedUser != GetLoggedUser())
	        {
                throw new BusinessException("Error", new List<string>() { string.Format("You do not have access to Assessment id = {0}.", assessmentId) });
	        }

            //IEnumerable<UserAnswer> answers = (from c in repository.RavenSession.Query<UserAnswer>()
            //                                where c.AssessmentId == RavenIdConverter.Convert(RavenIdPrefix.Assessments, testId)
            //                                select c).ToList();
            //var answersList = answers.Select(m => {
            //    var userInfo = repository.Load<User>(m.UserId);
            //    m.UserName = userInfo.FirstName + " " + userInfo.LastName;
            //    m.UserUniqueName = userInfo.Id;
            //    m.AssessmentName = repository.Load<Assessment>(m.AssessmentId).TestName;
            //    m.Id = m.Id.RemoveRavenIdPrefix(); 
            //    m.AssessmentId = m.AssessmentId.RemoveRavenIdPrefix();
            //    m.UserId = m.UserId.RemoveRavenIdPrefix();
            //    return m; 
            //}).ToList();
            //return answers;
            return null;
        }

        public UserAnswer GetUserAnswer(int id)
        {
            return repository.GetByID(id);
        }
        
        public UserAnswer AddAnswer(UserAnswer userAnswer)
        {
            UserAnswerValidator validator = new UserAnswerValidator();
            var results = validator.Validate(userAnswer);
            if (results.IsValid)
            {
                userAnswer.Id = 0;
                userAnswer.CreatedDate = userAnswer.ModifiedDate = DateTime.UtcNow;
                userAnswer.UserId = userAnswer.CreatedUser = userAnswer.ModifiedUser = GetLoggedUser();
                userAnswer.Status = AnswerStatus.InProgress;
                userAnswer.AssessmentId = userAnswer.AssessmentId;
                var assessment = GetAssessment(userAnswer.AssessmentId);
                if (assessment == null)
                {
                    throw new BusinessException("Invalid assessmentId = {0}".FormatWith(userAnswer.AssessmentId), new List<string> { "Invalid assessmentId = {0}".FormatWith(userAnswer.AssessmentId) });
                }
                if (userAnswer.Answers == null)
                {
                    userAnswer.Answers = new List<UserAnswer.Answer>();
                }
                else
                {
                    //check question belongs to the given assessment
                    
                    if (assessment != null && assessment.Questions != null && assessment.Questions.Length > 0)
                    {
                        foreach (var item in userAnswer.Answers)
                        {
                            var ans = item;
                            ValidateQuestionAnswer(assessment, ans);
                        }
                    }
                }
                userAnswer.AssessmentName = assessment.Name;
                userAnswer.TotalQuestions = assessment.Questions.Length;
                if (userAnswer.TotalQuestions > 0)
                {
                    var correctAnswersCount = userAnswer.Answers.Where(a => a.Result == ResultType.Correct).Count();
                    double percentage = ((double)correctAnswersCount / userAnswer.TotalQuestions) * 100;
                    userAnswer.Percentage = Math.Round(percentage, 2);
                }
                repository.Insert(userAnswer);
                var newAnswer = repository.GetByID(userAnswer.Id);
                return newAnswer;
            }
            else
            {
                var errors = results.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BusinessException(errors);
            }
        }

        private void ValidateQuestionAnswer(Assessment assessment, UserAnswer.Answer answer)
        {
            var id = answer.Question.Id;
            int questionReference = assessment.Questions.SingleOrDefault(x => x == id);
            if (questionReference == 0)
            {
                throw new BusinessException("Error", new List<string>() { string.Format("Question id = {0} not belongs to the given test.", answer.Question.Id) });
            }
            var question = questionManager.Get(id);
            if (question != null && question.Options != null)
            {
                //var option = question.Options.SingleOrDefault(o => o.Id.Equals(answer.SelectedOption, StringComparison.InvariantCultureIgnoreCase));
                //if (option == null)
                //{
                //    throw new BusinessException("Error", new List<string>() { string.Format("Selected Option '{0}' not belongs to the given Question id = {1}.", answer.SelectedOption, answer.Question.Id) });
                //}
                //answer.UserOptions = new List<Option>();
                //answer.UserOptions.Add(option);
                answer.ValidateAnswer(question.Options);
            }
            //answer.Question.Id = id;
        }
        
        public UserAnswer UpdateAnswer(UserAnswer userAnswer)
        {
            UserAnswerValidator validator = new UserAnswerValidator();
            var results = validator.Validate(userAnswer);
            if (!results.IsValid)
            {
                var errors = results.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BusinessException(errors);
            }

            var answerOrginal = repository.GetByID(userAnswer.Id);
            if (answerOrginal != null
                && answerOrginal.AssessmentId == userAnswer.AssessmentId
                && answerOrginal.UserId == GetLoggedUser())
            {
                if (answerOrginal.IsTestCompleted)
                {
                    throw new BusinessException("Error", new List<string>() { "Test already completed, your answers cannot be saved." });
                }
                var assessment = GetAssessment(answerOrginal.AssessmentId);
                if (assessment == null)
                {
                    throw new BusinessException("Error", new List<string>() { string.Format("No Assessment found with id = {0}.", userAnswer.AssessmentId) });
                }
                if (userAnswer.Answers == null)
                {
                    userAnswer.Answers = new List<UserAnswer.Answer>();
                }

                answerOrginal.ModifiedDate = DateTime.UtcNow;
                if (answerOrginal.CreatedDate.HasValue)
                {
                    var actualDuration = (int)DateTime.UtcNow.Subtract(answerOrginal.CreatedDate.Value.ToUniversalTime()).TotalSeconds;
                    if (actualDuration >= (assessment.Duration * 60))
                    {
                        answerOrginal.IsTestCompleted = true;
                        answerOrginal.TimeToComplete = assessment.Duration * 60;
                        answerOrginal.Status = AnswerStatus.Timeout;
                        answerOrginal.ModifiedUser = GetLoggedUser();
                        repository.Update(answerOrginal);

                        //throw new BusinessException("Error", new List<string>() { "Test timed out, your answers cannot be saved." });
                    }
                    answerOrginal.TimeToComplete = DateTime.UtcNow.Subtract(answerOrginal.CreatedDate.Value).TotalSeconds;

                    if (userAnswer.IsTestCompleted)
                    {
                        answerOrginal.Status = AnswerStatus.Completed;
                    }
                    else
                    {
                        answerOrginal.Status = AnswerStatus.InProgress;
                    }
                }
                answerOrginal.Comments = userAnswer.Comments;
                answerOrginal.IsTestCompleted = userAnswer.IsTestCompleted;
                answerOrginal.ModifiedUser = GetLoggedUser();

                if (userAnswer.Answers != null)
                {
                    var listNewAnswers = new List<UserAnswer.Answer>();
                    foreach (var item in userAnswer.Answers)
                    {
                        if (item != null && item.Question != null)
                        {
                            var ans = item;
                            //check question belongs to the given assessment.
                            if (assessment.Questions != null)
                            {
                                ValidateQuestionAnswer(assessment, ans);
                                var answer = answerOrginal.Answers.SingleOrDefault(x => x.Question.Id == ans.Question.Id);
                                if (answer != null)
                                {
                                    answer.UserOptions = ans.UserOptions;
                                    //answer.SelectedOption = ans.SelectedOption;
                                    answer.Question.Sort = ans.Question.Sort;
                                    answer.SetAnswer(ans.Result);
                                }
                                else
                                {
                                    listNewAnswers.Add(ans);
                                }
                            }
                        }
                    }
                    answerOrginal.Answers.AddRange(listNewAnswers);
                }
                if (answerOrginal.TotalQuestions > 0)
                {
                    var correctAnswersCount = answerOrginal.Answers.Where(a => a.Result == ResultType.Correct).Count();
                    double percentage = ((double)correctAnswersCount / answerOrginal.TotalQuestions) * 100;
                    answerOrginal.Percentage = Math.Round(percentage, 2);
                }
                repository.Update(answerOrginal);
            }
            else
            {
                throw new BusinessException("Error", new List<string>() { "user id is not mached, your answers cannot be saved." });
            }

            return userAnswer;
            
        }
        
        //private void UpdateAnswerTimeout(string answerId)
        //{
        //    var answer = repository.Load<UserAnswer>(answerId);

        //    if (answer.IsTestCompleted) return;

        //    if (answer.CreatedDate.HasValue)
        //    {
        //        var duration = GetAssessmentDuration(answer.AssessmentId);
        //        var actualDuration = (int)DateTime.UtcNow.Subtract(answer.CreatedDate.Value.ToUniversalTime()).TotalSeconds;
        //        if (actualDuration >= (duration * 60))
        //        {
        //            answer.IsTestCompleted = true;
        //            answer.TimeToComplete = duration * 60;
        //            repository.Save();
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assessmentId"></param>
        /// <returns>Assessment duration in minutes</returns>
        private Assessment GetAssessment(int assessmentId)
        {
            var assessment = assessmentManager.Get(assessmentId);
            if (assessment != null)
            {
                return assessment;
            }
            return null;
        }

        private void IsAssessmentTimeout(UserAnswer answer)
        {
            //check test has timeout, by assessment duration
            if (answer.IsTestCompleted)
            {
                return;
            }
            else
            {
                var assessment = GetAssessment(answer.AssessmentId);
                if (answer.CreatedDate.HasValue && assessment != null)
                {
                    var actualDuration = (int)DateTime.UtcNow.Subtract(answer.CreatedDate.Value.ToUniversalTime()).TotalSeconds;
                    if (actualDuration >= (assessment.Duration * 60))
                    {
                        answer.IsTestCompleted = true;
                        answer.TimeToComplete = assessment.Duration * 60;
                        answer.Status = AnswerStatus.Timeout;
                    }
                }
            }
        }

        /// <summary>
        /// Get UserAnswer without Prefix
        /// </summary>
        /// <param name="answer">Answer</param>
        /// <returns>Returns UserAnswer without id prefix</returns>
        //private UserAnswer GetAnswer(UserAnswer answer)
        //{
        //    if (answer.Answers != null && answer.Answers.Count > 0)
        //    {
        //        answer.Answers = answer.Answers.Select(x => { x.Question.QuestionId = x.Question.QuestionId; x.UserOptions = null; return x; }).ToList();
        //    }

        //    answer.Id = answer.Id;
        //    answer.AssessmentId = answer.AssessmentId;
        //    answer.UserId = answer.UserId;
        //    answer.CreatedUser = answer.CreatedUser;
        //    answer.ModifiedUser = answer.ModifiedUser;
            
        //    return answer;
        //}

        //private UserAnswer GetAnswer(string id)
        //{
        //    var answer = repository.Load<UserAnswer>(RavenIdConverter.Convert(RavenIdPrefix.UserAnswers, id));
        //    if (answer == null)
        //    {
        //        return null;
        //    }
        //    return GetAnswer(answer);
        //}
        
        public byte[] ExportToExcel(int testId)
        {
            var answers = GetAll(testId);

            ExcelPackage pck = new ExcelPackage();
            var ws = pck.Workbook.Worksheets.Add("TestResults");
            bool isFirstRow = true;
            int rowIdx = 2;

            if (answers != null && answers.Count() > 0)
            {
                //ExcelPackage pck = new ExcelPackage();
                //var ws = pck.Workbook.Worksheets.Add("test");
                foreach (var answer in answers)
                {
                    if (isFirstRow)
                    {
                        ws.Cells[1, 1].Value = "Assessment";
                        ws.Cells[1, 2].Value = "User";
                        ws.Cells[1, 3].Value = "Percentage";
                        ws.Cells[1, 4].Value = "Duration(min.)";
                        ws.Cells[1, 5].Value = "Start Date/Time";
                        ws.Cells[1, 6].Value = "Status";
                        ws.Cells[1, 1, 1, 6].Style.Font.Bold = true;
                    }

                    ws.Cells[rowIdx, 1].Value = answer.AssessmentName;
                    ws.Cells[rowIdx, 2].Value = answer.UserName;
                    ws.Cells[rowIdx, 3].Value = answer.Percentage;
                    ws.Cells[rowIdx, 4].Value = TimeSpan.FromSeconds(answer.TimeToComplete); //string.Format("{0:%m} min. {0:%s} sec.", TimeSpan.FromSeconds(answer.TimeToComplete));
                    ws.Cells[rowIdx, 4].Style.Numberformat.Format = "mm:ss";
                    ws.Cells[rowIdx, 5].Value = answer.CreatedDate;
                    ws.Cells[rowIdx, 5].Style.Numberformat.Format = "dd/mm/yyyy hh:mm:ss";
                    ws.Cells[rowIdx, 6].Value = answer.Status;

                    rowIdx++;
                }
            }
            else
                ws.Cells["A1"].Value = "No Data Found!";

            return pck.GetAsByteArray();
        }
        
        private string GetLoggedUser()
        {
            return "bpk";
            return TestBankIdentity.GetContextIdentity().UserIdentity.UserId;
        }
    }
}
