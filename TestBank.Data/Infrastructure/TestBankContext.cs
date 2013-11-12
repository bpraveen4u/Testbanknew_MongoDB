using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using TestBank.Entity;
using TestBank.Data.Mappings;

namespace TestBank.Data.Infrastructure
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new AssessmentMap());
        }
    }
}
