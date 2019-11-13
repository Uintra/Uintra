using System;
using System.Web.Mvc;
using IDependencyResolver = Compent.CommandBus.IDependencyResolver;

namespace Uintra20.Core.Ioc
{
    public class CommandBusDependencyResolver : IDependencyResolver
    {
        private readonly System.Web.Mvc.IDependencyResolver _dr;

        public CommandBusDependencyResolver(System.Web.Mvc.IDependencyResolver kernel)
        {
            _dr = kernel;
        }

        public T GetService<T>() => _dr.GetService<T>();

        public object GetService(Type type) => _dr.GetService(type);
    }
}