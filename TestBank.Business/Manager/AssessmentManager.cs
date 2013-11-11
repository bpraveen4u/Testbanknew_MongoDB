using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Data;
using TestBank.Entity;

namespace TestBank.Business.Manager
{
    public class AssessmentManager
    {
        private readonly IUnitOfWork UoW;

        public AssessmentManager(IUnitOfWork unitOfWork)
        {
            this.UoW = unitOfWork;
        }

        public PagedEntity<Assessment> GetAll(int page = 1, int pageSize = 10)
        {
            if (pageSize < 1)
                pageSize = 10;

            if (page < 1)
                page = 1;
            
            var totalRecords = UoW.AssessmentRepository.Get().Count();
            var pagedEntity = new PagedEntity<Assessment>()
            {
                TotalRecords = totalRecords,
                CurrentPage = page,
                TotalPages = Convert.ToInt32(Math.Ceiling((double) totalRecords / pageSize)),
                PageSize = pageSize,
                PagedData = UoW.AssessmentRepository.Get(page: page, pageSize: pageSize).ToList()
            };

            return pagedEntity;
        }

        public Assessment Insert(Assessment assessment)
        {
            UoW.AssessmentRepository.Insert(assessment);

            UoW.Commit();
            return assessment;
        }

        public Assessment Update(Assessment assessment)
        {
            UoW.AssessmentRepository.Update(assessment);

            UoW.Commit();
            return assessment;
        }

        public Assessment Get(int id)
        {
            return UoW.AssessmentRepository.GetByID(id);
        }

        public void Delete(int id)
        {
            UoW.AssessmentRepository.Delete(id);
            UoW.Commit();
        }
    }
}
