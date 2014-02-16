using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace TestBank.Entity.Sys
{
    public class TestBankPrincipal : IPrincipal
    {
        #region Private Fields
        private IIdentity _identity;
        private List<string> _roles;
        #endregion

        #region CTOR
        public TestBankPrincipal(IIdentity identity, List<string> roles)
        {
            _identity = identity;
            _roles = roles;
        }

        #endregion

        #region Props
        public IIdentity Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }

        public bool IsInRole(string role)
        {
            return _roles.Contains(role);
        }
        #endregion
    }
}
