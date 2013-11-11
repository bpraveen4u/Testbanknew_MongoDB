using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using TestBank.Data;
using TestBank.Business.Manager;

namespace TestBank.API.WebHost.App_Start
{
    public static class ServiceConfig
    {
        public static void RegisterServices(StandardKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<AssessmentManager>().ToSelf();
        }
    }
}