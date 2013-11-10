using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TestBank.API.WebHost.Infrastructure.DI;
using Ninject;
using TestBank.API.WebHost.App_Start;


namespace TestBank.API.WebHost
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            CreateKernel(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private IKernel CreateKernel(HttpConfiguration config)
        {
            var kernel = new StandardKernel();
            config.DependencyResolver = new NinjectResolver(kernel);
            ServiceConfig.RegisterServices(kernel);
            return kernel;
        }
    }
}