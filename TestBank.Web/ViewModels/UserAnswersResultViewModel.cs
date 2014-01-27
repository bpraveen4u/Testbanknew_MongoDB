using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.ComponentModel;

namespace TestBank.Web.ViewModels
{
    
    public class UserAnswersResultViewModel
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        [DisplayName("Assessment Name")]
        public string AssessmentName { get; set; }
        public string UserId { get; set; }
        public string UserUniqueName { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string Comments { get; set; }

        //public List<AnswerInputViewModel> QuestionAnswers { get; set; }
        //public IPagedList PagingMetaData { get; set; }
        //public int Duration { get; set; }
        [DisplayName("Time To Complete")]
        public string TimeToComplete { get; set; }
        //public int CurrentPageId { get; set; }
        [DisplayName("Start Date")]
        public DateTime? StartDateTime { get; set; }
        //public long JsTime { get; set; }
        public bool IsTestCompleted { get; set; }
        public string Status { get; set; }
        public double Percentage { get; set; }

        //public class AnswerInputViewModel
        //{
        //    public int Nr { get; set; }
        //    public int QuestionId { get; set; }
        //    public string Description { get; set; }
        //    public ICollection<SelectListItem> OptionItems { get; set; }
        //    public string SelectedOption { get; set; }
        //    //public List<QuestionViewModel.OptionViewModel> UserOptions { get; set; }
        //    //public ResultType Result { get; private set; }

        //    //public Answer()
        //    //{
        //    //    Result = ResultType.None;
        //    //}

        //}

    }

    
}