using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using TestBank.Data.Infrastructure;
using TestBank.Business.Manager;
using TestBank.Data.Repositories;
using TestBank.API.WebHost.Infrastructure.Logging;

namespace TestBank.API.WebHost.App_Start
{
    public static class ServiceConfig
    {
        public static void RegisterServices(StandardKernel kernel)
        {
            kernel.Bind<ILogger>().To<NLogLogger>();
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            //kernel.Bind<IAssessmentRepository>().To<AssessmentRepository>();
            //kernel.Bind<IQuestionRepository>().To<QuestionRepository>();
            kernel.Bind<IAssessmentRepository>().To<AssessmentMongoRepository>();
            kernel.Bind<IQuestionRepository>().To<QuestionMongoRepository>();
            //kernel.Bind<AssessmentManager>().ToSelf();
        }
    }
}