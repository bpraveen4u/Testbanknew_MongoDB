using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestBank.Entity.Sys;

namespace TestBank.Business.Infrastructure.Cache
{
    public static class CacheStore
    {
        public static Dictionary<string, TestBankPrincipal> Cache = new Dictionary<string, TestBankPrincipal>();
    }
}
