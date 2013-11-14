using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using TestBank.API.WebHost.Infrastructure.Logging;
using TestBank.Business.Exceptions;

namespace TestBank.API.WebHost.Infrastructure.Filters
{
    public class BusinessExceptionAttribute : ExceptionFilterAttribute
    {
        ILogger logger = LogFactory.Logger;
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var action = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            var badResponse = new HttpResponseMessage();

            if (actionExecutedContext.Exception is NotImplementedException)
            {
            }
            else if (actionExecutedContext.Exception is HttpResponseException)
            {
                throw actionExecutedContext.Exception;
            }
            else if (actionExecutedContext.Exception is BusinessException)
            {
                logger.Error((BusinessException)actionExecutedContext.Exception);
                var exception = actionExecutedContext.Exception as BusinessException;
                badResponse.StatusCode = HttpStatusCode.PreconditionFailed;
                if (exception.Errors != null)
                {
                    badResponse.Content = new ObjectContent(typeof(IEnumerable<string>), exception.Errors, new JsonMediaTypeFormatter());
                }
                else
                {
                    badResponse.Content = new ObjectContent(typeof(IEnumerable<string>), new string[] { exception.Message }, new JsonMediaTypeFormatter());
                }
            }
            else
            {
                logger.Fatal(actionExecutedContext.Exception);
                badResponse.Content = new StringContent("Server error occured. Please contact administrator");
                badResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            actionExecutedContext.Response = badResponse;

        }
    }
}