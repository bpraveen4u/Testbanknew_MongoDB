using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Data.Infrastructure;
using TestBank.Data.Repositories;
using TestBank.Entity;

namespace TestBank.Business.Manager
{
    public class AssessmentManager
    {
        private readonly IUnitOfWork UoW;
        private readonly IAssessmentRepository repository;

        public AssessmentManager(IUnitOfWork unitOfWork, IAssessmentRepository repository)
        {
            this.UoW = unitOfWork;
            this.repository = repository;
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
            repository.Insert(assessment);

            UoW.Commit();
            return assessment;
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
