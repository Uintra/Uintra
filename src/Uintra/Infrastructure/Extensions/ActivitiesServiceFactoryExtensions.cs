using System;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Entities;
using Uintra.Features.Notification.Services;

namespace Uintra.Infrastructure.Extensions
{
    public static class ActivitiesServiceFactoryExtensions
    {
        public static INotifyableService GetNotifyableService(
            this IActivitiesServiceFactory activitiesServiceFactory,
            Guid activityId) =>
            activitiesServiceFactory.GetService<INotifyableService>(activityId);

        public static ICacheableIntranetActivityService<IIntranetActivity> GetCacheableIntranetActivityService(
            this IActivitiesServiceFactory activitiesServiceFactory,
            Guid activityId) =>
            activitiesServiceFactory.GetService<ICacheableIntranetActivityService<IIntranetActivity>>(activityId);
    }
}