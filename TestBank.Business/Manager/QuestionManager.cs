using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Data.Infrastructure;
using TestBank.Data.Repositories;
using TestBank.Entity;
using TestBank.Business.Exceptions;
using TestBank.Business.Manager.Validator;

namespace TestBank.Business.Manager
{
    public class QuestionManager
    {
        private readonly IUnitOfWork UoW;
        private readonly IQuestionRepository repository;

        public QuestionManager(IUnitOfWork unitOfWork, IQuestionRepository repository)
        {
            this.UoW = unitOfWork;
            this.repository = repository;
        }

        public PagedEntity<Question> GetAll(int page = 1, int pageSize = 10)
        {
            if (pageSize < 1)
                pageSize = 10;

            if (page < 1)
                page = 1;

            var totalRecords = repository.Get().Count();
            var pagedEntity = new PagedEntity<Question>()
            {
                TotalRecords = totalRecords,
                CurrentPage = page,
                TotalPages = Convert.ToInt32(Math.Ceiling((double)totalRecords / pageSize)),
                PageSize = pageSize,
                PagedData = repository.Get(page: page, pageSize: pageSize).ToList()
            };

            return pagedEntity;
        }

        public Question Get(int id)
        {
            return repository.GetByID(id);
        }

        public Question Insert(Question question)
        {
            var validator = new QuestionValidator();
            var results = validator.Validate(question);
            if (results.IsValid)
            {
                repository.Insert(question);

                UoW.Commit();
                return question;
            }
            else
            {
                var errors = results.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BusinessException(errors);
            }
        }

    }
}
