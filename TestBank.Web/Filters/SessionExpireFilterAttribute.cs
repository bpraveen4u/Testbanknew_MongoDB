using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestBank.Web.Filters
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();

            bool sessionRequired = true;
            if (controllerName.Contains("account"))
            {
                sessionRequired = false;
            }
            else if (controllerName.Contains("home") && actionName == "index")
            {
                var apiKey = filterContext.HttpContext.Request.QueryString["apikey"];
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    filterContext.HttpContext.Session["APIKey"] = apiKey;
                    sessionRequired = false;
                }
            }
            else if (controllerName.Contains("answers") && actionName == "start")
            {
                sessionRequired = false;
            }

            if(sessionRequired)
            {
                var context = filterContext.HttpContext;
                if (context.Session != null)
                {
                    var apiKey = context.Session["APIKey"];
                    if (((apiKey == null) && (!context.Session.IsNewSession)) || (context.Session.IsNewSession))
                    {
                        string sessionCookie = context.Request.Headers["Cookie"];
                        if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                        {
                            ActionResult result = null;
                            if (context.Request.IsAjaxRequest())
                            {
                                result = new JsonResult { Data = new { LogonRequired = true } };
                            }
                            else
                            {
                                string redirectTo = "~/Account/Login";
                                if (!string.IsNullOrEmpty(context.Request.RawUrl))
                                {
                                    redirectTo = string.Format("~/Account/Login?ReturnUrl={0}",
                                    HttpUtility.UrlEncode(context.Request.RawUrl));
                                }
                                result = new RedirectResult(redirectTo);
                            }
                            filterContext.Result = result;
                        }
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}