using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestBank.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public string ApiKey {
            get
            {
                return Convert.ToString(Session["APIKey"]);
            }
            set
            {
                Session["APIKey"] = value;
            }
        }

    }
}
