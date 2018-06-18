using System;
using System.Web.Mvc;
using IDependencyResolver = Compent.CommandBus.IDependencyResolver;

namespace Compent.Uintra.Core.IoC
{
    public class CommandBusDependencyResolver : IDependencyResolver
    {
        private readonly System.Web.Mvc.IDependencyResolver _kernel;

        public CommandBusDependencyResolver(System.Web.Mvc.IDependencyResolver kernel)
        {
            _kernel = kernel;
        }

        public T GetService<T>() => _kernel.GetService<T>();

        public object GetService(Type type) => _kernel.GetService(type);
    }
}