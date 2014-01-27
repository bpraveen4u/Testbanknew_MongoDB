using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestBank.Web.Filters
{
    [Serializable]
    [Flags]
    public enum SiteRoles
    {
        User = 0,
        Admin = 1,
        Helpdesk = 2
    }

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public new SiteRoles Roles;
        private bool FailedRolesAuth = false;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //string[] users = Users.Split(',');

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            //if (users.Length > 0 &&
            //    !users.Contains(httpContext.User.Identity.Name,
            //        StringComparer.OrdinalIgnoreCase))
            //    return false;

            //SiteRoles role = (SiteRoles)httpContext.Session["role"];

            //if (Roles != 0 && (Roles & role) != role)
            //{
            //    FailedRolesAuth = true;
            //    return false;
            //}

            return true;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (FailedRolesAuth)
            {
                filterContext.Result = new ViewResult { ViewName = "NotAuth" };
            }
        }
    }
}