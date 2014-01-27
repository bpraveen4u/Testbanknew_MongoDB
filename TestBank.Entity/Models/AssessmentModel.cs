using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBank.Entity.Models
{
    public class AssessmentModel
    {
        public ICollection<LinkModel> Links { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Duration { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public bool Enable { get; set; }
    }

    public class AssessmentDetailsModel : AssessmentModel
    {
        //public ICollection<LinkModel> Links { get; set; }
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public double Duration { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public string ModifiedUser { get; set; }
        //public bool Enable { get; set; }
        public int Sort { get; set; }
        public List<QuestionModel> Questions { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string Link { get; set; }
        public string ShortLink { get; set; }
        public string Status { get; set; }
        public int MaxOptions { get; set; }
    }
}