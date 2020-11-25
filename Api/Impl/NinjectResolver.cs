using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;

namespace Wallet.GASender.Api.Impl
{
    public class NinjectResolver : IDependencyResolver, IDependencyScope
    {
        public IKernel Kernel { get; private set; }

        public NinjectResolver()
        {
            Kernel = new StandardKernel();
        }

        public object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose() { }
    }
}
