﻿using System;
using System.Linq;
using System.Web.Mvc;

namespace Uintra20.Core.Activity
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
            return GetService<TService>(activityType);
        }

        public TService GetService<TService>(Enum type) where TService : class, ITypedService
        {
            return _kernel.GetServices<TService>().SingleOrDefault(s => Equals(s.Type, type));
        }
    }
}