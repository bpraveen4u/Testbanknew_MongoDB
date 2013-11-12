using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBank.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        TestBankContext Get();
    }
}
