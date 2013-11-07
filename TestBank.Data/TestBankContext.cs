using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using TestBank.Entity;

namespace TestBank.Data
{
    public class TestBankContext: DbContext
    {
        public TestBankContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
    }
}
