using System;
using Uintra.Core.Activity;
using Uintra.Notification;

namespace Compent.Uintra.Core.Activity
{
    public static class ActivitiesServiceFactoryExtensions
    {
        public static INotifyableService GetNotifyableService(this IActivitiesServiceFactory activitiesServiceFactory, Guid activityId)
        {
            return activitiesServiceFactory.GetService<INotifyableService>(activityId);
        }

        public static ICacheableIntranetActivityService<IIntranetActivity> GetCacheableIntranetActivityService(this IActivitiesServiceFactory activitiesServiceFactory, Guid activityId)
        {
            return activitiesServiceFactory.GetService<ICacheableIntranetActivityService<IIntranetActivity>>(activityId);
        }
    }
}