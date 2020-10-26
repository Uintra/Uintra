using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UBaseline.Core.Extensions;
using Uintra20.Core.Activity.Helpers;
using Umbraco.Core.Composing;

namespace Uintra20.Core.Activity
{
    public class ActivitiesServiceFactory : IActivitiesServiceFactory
    {
        private readonly IActivityTypeHelper _activityTypeHelper;

        public ActivitiesServiceFactory(IActivityTypeHelper activityTypeHelper)
        {
            _activityTypeHelper = activityTypeHelper;
        }

        public TService GetService<TService>(Guid activityId) where TService : class, ITypedService
        {
            var activityType = _activityTypeHelper.GetActivityType(activityId);
            return GetService<TService>(activityType);
        }

        public TService GetService<TService>(Enum type) where TService : class, ITypedService
        {
            var services= Current.Factory.EnsureScope(s=>(IEnumerable<TService>)s.GetAllInstances(typeof(TService)));
            return services.FirstOrDefault(s => Equals(s.Type, type));
        }

        public IEnumerable<TService> GetServices<TService>() where TService : class, ITypedService
        {
            return DependencyResolver.Current.GetServices<TService>();
        }
    }
}