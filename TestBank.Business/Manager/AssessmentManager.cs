using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Business.Exceptions;
using TestBank.Business.Manager.Validator;
using TestBank.Data.Infrastructure;
using TestBank.Data.Repositories;
using TestBank.Entity;

namespace TestBank.Business.Manager
{
    public class AssessmentManager
    {
        private readonly IUnitOfWork UoW;
        private readonly IAssessmentRepository repository;
        private readonly IQuestionRepository questionRepository;

        public AssessmentManager(IUnitOfWork unitOfWork, IAssessmentRepository repository, IQuestionRepository questionRepository)
        {
            this.UoW = unitOfWork;
            this.repository = repository;
            this.questionRepository = questionRepository;
        }

        public PagedEntity<Assessment> GetAll(int page = 1, int pageSize = 10)
        {
            if (pageSize < 1)
                pageSize = 10;

            if (page < 1)
                page = 1;
            
            var totalRecords = repository.Get().Count();
            var pagedEntity = new PagedEntity<Assessment>()
            {
                TotalRecords = totalRecords,
                CurrentPage = page,
                TotalPages = Convert.ToInt32(Math.Ceiling((double) totalRecords / pageSize)),
                PageSize = pageSize,
                PagedData = repository.Get(page: page, pageSize: pageSize).ToList()
            };

            return pagedEntity;
        }

        public Assessment Insert(Assessment assessment)
        {
            AssessmentValidator validator = new AssessmentValidator();
            var results = validator.Validate(assessment);
            if (results.IsValid)
            {
                repository.Insert(assessment);

                UoW.Commit();
                return assessment;
            }
            else
            {
                var errors = results.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BusinessException(errors);
            }
        }

        public Assessment Update(Assessment assessment)
        {
            repository.Update(assessment);

            UoW.Commit();
            return assessment;
        }

        public Assessment Get(int id)
        {
            return repository.GetByID(id);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
            UoW.Commit();
        }
    }
}
