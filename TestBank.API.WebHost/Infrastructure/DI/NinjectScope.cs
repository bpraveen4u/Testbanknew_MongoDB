using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Ninject.Syntax;
using Ninject.Activation;
using Ninject.Parameters;

namespace TestBank.API.WebHost.Infrastructure.DI
{
    public class NinjectScope : IDependencyScope
    {
        //kernel
        protected IResolutionRoot resolutionRoot;
        //overloaded constrctor
        public NinjectScope(IResolutionRoot kernel)
        {
            this.resolutionRoot = kernel;
        }

        public object GetService(Type serviceType)
        {
            IRequest request = resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return resolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IRequest request = resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return resolutionRoot.Resolve(request).ToList();
        }

        public void Dispose()
        {
            IDisposable disposable = resolutionRoot as IDisposable;
            if (disposable != null) disposable.Dispose();
            resolutionRoot = null;
        }
    }
}