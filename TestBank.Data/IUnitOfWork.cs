using System;
using TestBank.Entity;
namespace TestBank.Data
{
    public interface IUnitOfWork
    {
        GenericRepository<Assessment> AssessmentRepository { get; }
        GenericRepository<Option> OptionRepository { get; }
        GenericRepository<Question> QuestionRepository { get; }
        void Commit();
    }
}
