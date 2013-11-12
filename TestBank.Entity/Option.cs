using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Entity
{
    public class Option : IEntity
    {
        public int Id { get; set; }
        public string Nr { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }
        public virtual Question Question { get; set; }
    }
}
