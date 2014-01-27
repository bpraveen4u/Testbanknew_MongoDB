using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TestBank.Entity.Models
{
    [DefaultValue("NotStarted")]
    public enum AnswerStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        Timeout = 3
    }
    [DefaultValue("None")]
    public enum ResultType
    {
        None = 0,
        Correct = 1,
        Wrong = 2,
        PartialCorrect = 3
    }
    public class UserAnswerModel //: IModel
    {
        public string Id { get; set; }
        public int Sort { get; set; }
        public string AssessmentId { get; set; }
        public string AssessmentName { get; set; }
        public string UserId { get; set; }
        public string UserUniqueName { get; set; }
        public string UserName { get; set; }
        public string Comments { get; set; }
        public List<Answer> Answers { get; set; }
        public double TimeToComplete { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public AnswerStatus Status { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }

        /// <summary>
        /// Set the user test has been finished or timedout
        /// </summary>
        public bool IsTestCompleted { get; set; }
        
        public class Answer
        {
            //public QuestionReference Question { get; set; }
            public List<Option> UserOptions { get; set; }
            public ResultType Result { get; private set; }
            public string SelectedOption { get; set; }

            public Answer()
            {
                Result = ResultType.None;
            }

            public void SetAnswer(ResultType result)
            {
                Result = result;
            }

            public void ValidateAnswer(List<Option> questionOptions)
            {
                //TO-DO logic to validate the user options aginst the question correct options
                if (questionOptions != null && (UserOptions != null && UserOptions.Count > 0))
                {
                    foreach (var opt in UserOptions)
                    {
                        switch (opt.Type)
                        {
                            case OptionType.None:
                                Result = ResultType.None;
                                break;
                            case OptionType.RadioButton:
                                
                                    if (opt.IsCorrect == true)
                                    {
                                        Result = ResultType.Correct;
                                    }
                                    else
                                        Result = ResultType.Wrong;
                                break;
                            case OptionType.CheckBox:
                                foreach (var correctOpt in questionOptions.Where(o => o.IsCorrect == true))
                                {
                                    if (true)
                                    {

                                    }
                                }
                                break;
                            case OptionType.DropDown:
                                break;
                            case OptionType.Text:
                                break;
                            default:
                                break;
                        }
                        
                    }
                }
                
            }
        }
        
    }
}
