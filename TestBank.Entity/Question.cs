using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity
{
    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public virtual List<Option> Options { get; set; }
    }
}
