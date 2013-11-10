using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity;

namespace TestBank.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private TestBankContext context = new TestBankContext();
        private GenericRepository<Assessment> assessmentRepository;
        private GenericRepository<Question> questionRepository;
        private GenericRepository<Option> optionRepository;

        //public UnitOfWork(TestBankContext context)
        //{
        //    this.context = context;
        //}

        public GenericRepository<Assessment> AssessmentRepository 
        {
            get
            {
                if (assessmentRepository == null)
                {
                    this.assessmentRepository = new GenericRepository<Assessment>(context);
                }
                return assessmentRepository;
            }
        }

        public GenericRepository<Question> QuestionRepository
        {
            get
            {
                if (questionRepository == null)
                {
                    this.questionRepository = new GenericRepository<Question>(context);
                }
                return questionRepository;
            }
        }

        public GenericRepository<Option> OptionRepository
        {
            get
            {
                if (optionRepository == null)
                {
                    this.optionRepository = new GenericRepository<Option>(context);
                }
                return optionRepository;
            }
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
