using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentValidation.Mvc;
using TestBank.Web.Infrastructure.ModelBinder;
using System.Web.Security;
using TestBank.Web.Infrastructure.Security;
using TestBank.Entity.Models;
using System.Security.Principal;
using TestBank.Web.Infrastructure.ValueProviders;

namespace TestBank.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            ValueProviderFactories.Factories.Insert(3, new CryptoValueProviderFactory()); //Its need to be inserted before the QueryStringValueProviderFactory
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            FluentValidationModelValidatorProvider.Configure();
            TestBank.Web.Infrastructure.AutoMapper.AutoMapperConfiguration.Configure();
            FormatterConfig.RegisterGlobalFormatters(GlobalConfiguration.Configuration.Formatters);
            ModelBinders.Binders.DefaultBinder = new DefaultGraphModelBinder();

            //ModelBinders.Binders.Add(typeof(CryptoValueProvider), new CrptoValueProviderDefaultModelBinder(typeof(CryptoValueProvider)));
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new CustomTestBankIdentity(new UserModel() { Id = authTicket.Name });
                var principal = new GenericPrincipal(identity, new string[] { "" });
                Context.User = principal;
            }
        }
    }
}