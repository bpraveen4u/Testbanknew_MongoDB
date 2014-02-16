using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Threading;
using TestBank.Entity.Models;

namespace TestBank.Entity.Sys
{
    public class TestBankIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get;
            set;
        }

        public bool IsAuthenticated
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Controller { get; set; }
        public string Action { get; set; }
        public string UserIP { get; set; }
        public string UserAgent { get; set; }
        public string RequestedParams { get; set; }
        public int LocaleId { get; set; }
        public UserIdentity UserIdentity { get; set; }

        public TestBankIdentity()
        {
            this.IsAuthenticated = false;
            this.LocaleId = 0;
            this.Name = string.Empty;
            this.UserIdentity = new UserIdentity();

            this.Controller = string.Empty;
            this.Controller = string.Empty;
            this.UserIP = string.Empty;
            this.UserAgent = string.Empty;
            this.RequestedParams = string.Empty;
        }

        public static TestBankIdentity GetContextIdentity()
        {
            return (Thread.CurrentPrincipal as TestBankPrincipal)
                    .Identity as TestBankIdentity;
        }
    }
}
