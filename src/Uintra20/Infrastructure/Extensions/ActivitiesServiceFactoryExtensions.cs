using System;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Infrastructure.Extensions
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