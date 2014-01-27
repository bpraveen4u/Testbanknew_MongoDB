using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using FluentValidation.Attributes;
using TestBank.Web.ViewModels.Validations;

namespace TestBank.Web.ViewModels
{
    public enum Status
    {
        None = 0,
        Started = 1,
        Completed = 2,
    }

    [Validator(typeof(AssessmentViewModelValidator))]
    public class AssessmentViewModel
    {
        public int Id { get; set; }
        public int Sort { get; set; }
        [DisplayName("Name")]
        public string TestName { get; set; }
        //public List<QuestionReferenceViewModel> AllQuestions { get; set; }

        public List<QuestionReferenceViewModel> Questions { get; set; }
        //public int[] AllQuestionIds { get; set; }
        public int[] QuestionIds { get; set; }
        public string Link { get; set; }
        public string ShortLink { get; set; }
        public Status Status { get; set; }
        public bool Enable { get; set; }
        [DisplayName("Duration (in minutes)")]
        public int Duration { get; set; }
        public int MaxOptions { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
    }

}