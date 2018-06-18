using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace Compent.Uintra.Core.IoC
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IResolutionRoot resolver;

        public NinjectDependencyResolver(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return resolver.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}