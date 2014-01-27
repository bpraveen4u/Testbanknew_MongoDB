using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using TestBank.Entity.Models;

namespace TestBank.Web.Infrastructure.Security
{
    public class CustomTestBankIdentity : IIdentity
    {
        public UserModel User { get; private set; }

        public CustomTestBankIdentity(UserModel user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
 
            this.User = user;
        }

        public string AuthenticationType
        {
            get { return "CustomeTestBank"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return User.Id; }
        }
    }
}