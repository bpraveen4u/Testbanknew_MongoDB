using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity
{
    [CollectionNameAttribute("questions")]
    public class Question : IEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<Option> Options { get; set; }

        public int Sort { get; set; }
        public string InstructorRemarks { get; set; }
        public byte Weightage { get; set; }
        public float CorrectScore { get; set; }
        public float WrongScore { get; set; }

        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
