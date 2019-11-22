using System;
using System.Web.Mvc;
using IDependencyResolver = Compent.CommandBus.IDependencyResolver;

namespace Uintra20.Infrastructure.Ioc
{
    public class CommandBusDependencyResolver : IDependencyResolver
    {
        public T GetService<T>() => DependencyResolver.Current.GetService<T>();

        public object GetService(Type type) => DependencyResolver.Current.GetService(type);
    }
}