using System;
using TestBank.Entity;
namespace TestBank.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        //RepositoryBase<Assessment> AssessmentRepository { get; }
        //RepositoryBase<Option> OptionRepository { get; }
        //RepositoryBase<Question> QuestionRepository { get; }
        void Commit();
    }
}
