using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestBank.Web.ViewModels
{
    public class ErrorViewModel : HandleErrorInfo
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorViewModel(Exception exception, string controllerName, string actionName)
            : base(exception, controllerName, actionName)
        {

        }
    }
}