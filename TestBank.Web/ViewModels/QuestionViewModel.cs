using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using TestBank.Web.ViewModels.Validations;
using FluentValidation.Attributes;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TestBank.Web.ViewModels
{
    [DefaultValue("RadioButton")]
    public enum QuestionOptionType
    {
        None = 0,
        RadioButton = 1,
        CheckBox = 2,
        DropDown = 3,
        Text = 4
    }

    [Validator(typeof(QuestionViewModelValidator))]
    public class QuestionViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public int Nr { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int Sort { get; set; }
        [DisplayName("Instructor Remarks")]
        public string InstructorRemarks { get; set; }
        public string Category { get; set; }
        public List<OptionViewModel> Options { get; set; }
        public byte Weightage { get; set; }
        [DisplayName("Correct Score")]
        public float CorrectScore { get; set; }
        [DisplayName("Wrong Score")]
        public float WrongScore { get; set; }
        
        [Validator(typeof(OptionViewModelValidator))]
        public class OptionViewModel
        {
            public string Id { get; set; }
            public int Sort { get; set; }
            public QuestionOptionType Type { get; set; }
            public string Text { get; set; }
            [DisplayName("Is Correct?")]
            public bool IsCorrect { get; set; }
            //public List<OptionViewModel> Options { get; set; }
        }
    }

    public class QuestionReferenceViewModel
    {
        public int QuestionId { get; set; }
        public string Description { get; set; }
        public int Sort { get; set; }
    }

    //public class QuestionReferenceInputViewModel
    //{
    //    public string Id { get; set; }
    //    public string Description { get; set; }
    //    public int Sort { get; set; }
    //}

}