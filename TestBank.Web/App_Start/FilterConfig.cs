using System.Web;
using System.Web.Mvc;
using TestBank.Web.Filters;
using System.Web.Http.Filters;

namespace TestBank.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new TestError());
            //filters.Add(new AuthorizeUserAttribute());
            filters.Add(new SessionExpireFilterAttribute());
        }

        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new ApiErrorAttribute());
        }

    }
}