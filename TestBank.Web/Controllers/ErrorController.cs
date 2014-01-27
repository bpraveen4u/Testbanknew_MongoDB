using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestBank.Infrastructure.Extensions;

namespace TestBank.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound(string url)
        {
            var originalUri = url ?? Request.QueryString["aspxerrorpath"] ?? Request.Url.OriginalString;

            var controllerName = (string)RouteData.Values["controller"];
            var actionName = (string)RouteData.Values["action"];
            var model = new NotFoundModel(new HttpException(404, "Failed to find page"), controllerName, actionName)
            {
                RequestedUrl = originalUri,
                ReferrerUrl = Request.UrlReferrer == null ? "" : Request.UrlReferrer.OriginalString
            };

            Response.StatusCode = 404;
            return View("NotFound", model);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            var name = GetViewName(ControllerContext, "~/Views/Error/{0}".FormatWith(actionName),
                                                        "~/Views/Error/Error",
                                                        "~/Views/Error/General",
                                                        "~/Views/Shared/Error");

            var controllerName = (string)RouteData.Values["controller"];
            var model = new HandleErrorInfo(Server.GetLastError(), controllerName, actionName);
            var result = new ViewResult
            {
                ViewName = name,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
            };

            Response.StatusCode = 501;
            result.ExecuteResult(ControllerContext);
        }

        protected string GetViewName(ControllerContext context, params string[] names)
        {
            foreach (var name in names)
            {
                var result = ViewEngines.Engines.FindView(ControllerContext, name, null);
                if (result.View != null)
                    return name;
            }
            return null;
        }
    }

    public class NotFoundModel : HandleErrorInfo
    {
        public NotFoundModel(Exception exception, string controllerName, string actionName)
            : base(exception, controllerName, actionName)
        {
        }
        public string RequestedUrl { get; set; }
        public string ReferrerUrl { get; set; }
    }
    
}
