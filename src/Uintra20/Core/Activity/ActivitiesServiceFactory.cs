using System;
using System.Linq;
using System.Web.Mvc;
using Uintra20.Core.Activity.Helpers;

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
            return DependencyResolver.Current.GetServices<TService>().SingleOrDefault(s => Equals(s.Type, type));
        }
    }
}