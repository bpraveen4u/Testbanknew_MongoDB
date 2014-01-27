using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestBank.Entity;

namespace TestBank.Entity.Models
{
    public class QuestionModel
    {
        public ICollection<LinkModel> Links { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Sort { get; set; }
        public List<OptionModel> Options { get; set; }
    }

    public class QuestionDetailsModel : QuestionModel
    {
        
        
        public string InstructorRemarks { get; set; }
        public byte Weightage { get; set; }
        public float CorrectScore { get; set; }
        public float WrongScore { get; set; }

        public string CreatedUser { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class OptionModel
    {
        public ICollection<LinkModel> Links { get; set; }
        public string Id { get; set; }
        public OptionType Type { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }
    }

}