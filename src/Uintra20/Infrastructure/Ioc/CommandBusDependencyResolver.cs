using System;
using System.Web.Mvc;

namespace Uintra20.Infrastructure.Ioc
{
    public class CommandBusDependencyResolver : Compent.CommandBus.IDependencyResolver
    {
        public T GetService<T>() =>
            DependencyResolver.Current.GetService<T>();

        public object GetService(Type type) =>
            DependencyResolver.Current.GetService(type);
    }
}