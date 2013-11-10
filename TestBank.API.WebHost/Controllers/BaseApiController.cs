using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestBank.Data;
using TestBank.API.WebHost.Models;

namespace TestBank.API.WebHost.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        private UnitOfWork unitOfWork;
        private ModelFactory modelFactory;
        public BaseApiController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public UnitOfWork UnitOfWork 
        {
            get
            {
                return unitOfWork;
            }
        }
        protected ModelFactory TheModelFactory
        {
            get
            {
                if (modelFactory == null)
                {
                    modelFactory = new ModelFactory(this.Request, unitOfWork);
                }
                return modelFactory;
            }
        }
    }
}
