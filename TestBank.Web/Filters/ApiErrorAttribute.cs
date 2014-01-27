using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using TestBank.Entity.Errors;
using System.Web.Mvc;
using TestBank.Web.ViewModels;
using System.Web.Routing;
using NLog;

namespace TestBank.Web.Filters
{
    public class TestError : System.Web.Mvc.HandleErrorAttribute
    {
        //ILogger
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                OnAjaxException(filterContext);
            }
            else
            {
                OnRegularException(filterContext);
            }
        }

        internal protected void OnRegularException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;

            Logger logger = LogManager.GetLogger("*");
            logger.ErrorException(exception.Message, exception);

            filterContext.HttpContext.Response.Clear();
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            ErrorViewModel model = new ErrorViewModel(exception, controllerName , actionName );
            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
            };
            filterContext.ExceptionHandled = true;
        }

        internal protected void OnAjaxException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;

            //ILog logger = LogManager.GetLogger(LoggerType);
            //logger.Error(Resources.Error.UnhandledAjaxException, exception);

            //filterContext.HttpContext.Response.Clear();
            //filterContext.HttpContext.Response.Status = Resources.Constants.HttpSuccess;

            //string errorMessage = WebUtility.GetUserExceptionMessage(exception, true);

            //filterContext.Result = new ExceptionJsonResult(new[] { errorMessage });
            //filterContext.ExceptionHandled = true;
        }

        protected string GetViewName(ControllerContext context, params string[] names)
        {
            foreach (var name in names)
            {
                var result = ViewEngines.Engines.FindView(context, name, null);
                if (result.View != null)
                    return name;
            }
            return null;
        }
    }

    public class ApiErrorAttribute : ExceptionFilterAttribute
    {
        //public override void OnException(ExceptionContext actionExecutedContext)
        //{
        //    base.OnException(filterContext);
        //}
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //var controller = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            //var action = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            var badResponse = new HttpResponseMessage();

            if (actionExecutedContext.Exception is ApiError)
            {
                ApiError error = actionExecutedContext.Exception as ApiError;
                foreach (var item in error.Errors)
                {
                    actionExecutedContext.ActionContext.ModelState.AddModelError("", item);
                }
            }
            else if (actionExecutedContext.Exception is NotImplementedException)
            {
                //_logger.Error(actionExecutedContext.Exception);
                badResponse = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                badResponse.Content = new StringContent("Request method is Not implemented.");
            }
            else if (actionExecutedContext.Exception is HttpResponseException)
            {
                throw actionExecutedContext.Exception;
            }
            else if (actionExecutedContext.Exception is HttpResponseException)
            {
                //_logger.Error((HttpResponseException)actionExecutedContext.Exception);
            }
            else
            {
                //_logger.Fatal(actionExecutedContext.Exception);
                badResponse.Content = new StringContent("Server error occured. Please contact administrator");
                badResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            actionExecutedContext.Response = badResponse;
            
        }

        public System.Threading.Tasks.Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public bool AllowMultiple
        {
            get { throw new NotImplementedException(); }
        }
    }
}