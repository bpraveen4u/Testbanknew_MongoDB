using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBank.API.WebHost.Models
{
    public class AssessmentModel
    {
        public ICollection<LinkModel> Links { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
    }
}