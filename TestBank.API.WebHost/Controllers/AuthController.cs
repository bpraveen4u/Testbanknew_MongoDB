using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using TestBank.Entity.Models;
using System.Net;
using TestBank.Entity.Sys;
using System.Threading;
using TestBank.Business.Manager;

namespace TestBank.Business.Controllers
{
    public class AuthController: ApiController
    {
        private const string APIKEY = "APIKey";
        private readonly UsersManager manager;

        public AuthController(UsersManager manager)
        {
            this.manager = manager;
        }

        [AllowAnonymous]
        public HttpResponseMessage Login(Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                var identity = manager.ValidateUserLogin(credentials);
                if (identity != null)
                {
                    identity.IsAuthenticated = true;
                    //populate Acl's and assign to identity object
                    TestBankPrincipal principal = new TestBankPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;
                    TestBank.Business.Infrastructure.Cache.CacheStore.Cache.Add(identity.UserIdentity.ApiKey.ToString(), principal);
                    var response = Request.CreateResponse<string>(HttpStatusCode.OK, identity.UserIdentity.ApiKey);
                    response.Headers.Add(APIKEY, identity.UserIdentity.ApiKey.ToString());
                    //logger.Info("successfully user:'{0}' is authenticated.".FormatWith(identity.Name));
                    return response;
                }
                var badResponse = new HttpResponseMessage();
                badResponse.StatusCode = HttpStatusCode.Unauthorized;
                badResponse.Content = new StringContent("Invalid user name or password.");
                throw new HttpResponseException(badResponse);
            }
            else
            {
                var badResponse = new HttpResponseMessage();
                badResponse.StatusCode = HttpStatusCode.PreconditionFailed;
                badResponse.Content = new StringContent("Invalid user name or password.");
                throw new HttpResponseException(badResponse);
            }
        }

        [HttpPut]
        public HttpResponseMessage Logout()
        {
            var testbankIdentity = (Thread.CurrentPrincipal as TestBankPrincipal).Identity as TestBankIdentity;

            TestBank.Business.Infrastructure.Cache.CacheStore.Cache.Remove(testbankIdentity.UserIdentity.ApiKey.ToString());
            
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
