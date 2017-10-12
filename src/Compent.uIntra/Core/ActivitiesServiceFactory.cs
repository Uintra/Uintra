using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Compent.uIntra.Core.Bulletins;
using Compent.uIntra.Core.Events;
using uIntra.Bulletins;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Events;
using uIntra.News;

namespace Compent.uIntra.Core
{
    public class ActivitiesServiceFactory : IActivitiesServiceFactory
    {
        private readonly IDependencyResolver _kernel;
        private readonly IActivityTypeHelper _activityTypeHelper;
        private readonly ICacheService _cache;

        public ActivitiesServiceFactory(
            IDependencyResolver kernel,
            IActivityTypeHelper activityTypeHelper,
            ICacheService cache)
        {
            _kernel = kernel;
            _activityTypeHelper = activityTypeHelper;
            _cache = cache;
        }

        public TService GetService<TService>(Guid id) where TService : ITypedService
        {
            var activityType = _activityTypeHelper.GetActivityType(id);
            return GetService<TService>(activityType.Id);
        }
       
        public TService GetService<TService>(int typeId) where TService : ITypedService
        {
            return _cache
                .GetOrSet(GetCacheKey<TService>(), ResolveServices<TService>,DateTimeOffset.Now)
                .Single(s => s.ActivityType.Id == typeId);
        }

        private IEnumerable<TService> ResolveServices<TService>() where TService : ITypedService
        {
            var serviceType = typeof(TService);
             return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => serviceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .Select(CreateInstance<TService>);
        }

        private TService CreateInstance<TService>(Type type)
        {
            var dependencies = type
                .GetConstructors()
                .FirstOrDefault()?
                .GetParameters()
                .Select(p => _kernel.GetService(p.ParameterType))
                .ToArray();

            return (TService) Activator.CreateInstance(type, dependencies);
        }

        private string GetCacheKey<TService>() => typeof(TService).Name;
    }
}
