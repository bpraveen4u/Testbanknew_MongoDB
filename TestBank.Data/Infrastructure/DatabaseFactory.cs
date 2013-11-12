using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private TestBankContext dataContext;
        public TestBankContext Get()
        {
            return dataContext ?? (dataContext = new TestBankContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
