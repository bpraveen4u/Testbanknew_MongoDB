using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace TestBank.Web.ViewModels
{
    public enum ResultType
    {
        None = 0,
        Correct = 1,
        Wrong = 2,
        PartialCorrect = 3
    }
    //[Bind(Exclude = "QuestionAnswers")]
    public class UserAnswersInputViewModel
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public string UserId { get; set; }
        public string Comments { get; set; }
        public List<AnswerInputViewModel> QuestionAnswers { get; set; }
        public IPagedList PagingMetaData { get; set; }
        public int Duration { get; set; }
        public double TimeToComplete { get; set; }
        public int CurrentPageId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public long JsTime { get; set; }
        public bool IsTestCompleted { get; set; }

        public class AnswerInputViewModel
        {
            public int Nr { get; set; }
            public int QuestionId { get; set; }
            public string Description { get; set; }
            public ICollection<SelectListItem> OptionItems { get; set; }
            public string SelectedOption { get; set; }
            //public List<QuestionViewModel.OptionViewModel> UserOptions { get; set; }
            //public ResultType Result { get; private set; }

            //public Answer()
            //{
            //    Result = ResultType.None;
            //}

        }

    }

    
}