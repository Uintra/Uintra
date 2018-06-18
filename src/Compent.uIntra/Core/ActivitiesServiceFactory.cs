using System;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.Activity;

namespace Compent.uIntra.Core
{
    public class ActivitiesServiceFactory : IActivitiesServiceFactory
    {
        private readonly IDependencyResolver _kernel;
        private readonly IActivityTypeHelper _activityTypeHelper;

        public ActivitiesServiceFactory(
            IDependencyResolver kernel,
            IActivityTypeHelper activityTypeHelper)
        {
            _kernel = kernel;
            _activityTypeHelper = activityTypeHelper;
        }

        public TService GetService<TService>(Guid activityId) where TService : class, ITypedService
        {
            var activityType = _activityTypeHelper.GetActivityType(activityId);
            return GetService<TService>(activityType.Id);
        }

        public TService GetService<TService>(int typeId) where TService : class, ITypedService
        {
            return _kernel.GetServices<TService>().Single(s => s.ActivityType.Id == typeId);
        }
    }
}
