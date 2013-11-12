using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using TestBank.Data.Infrastructure;
using TestBank.Business.Manager;
using TestBank.Data.Repositories;

namespace TestBank.API.WebHost.App_Start
{
    public static class ServiceConfig
    {
        public static void RegisterServices(StandardKernel kernel)
        {
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<IAssessmentRepository>().To<AssessmentRepository>();
            //kernel.Bind<AssessmentManager>().ToSelf();
        }
    }
}