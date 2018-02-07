using System;

namespace uIntra.Core.Activity
{
    public interface IActivitiesServiceFactory
    {
        TService GetService<TService>(Guid activityId) where TService : class, ITypedService;
        TService GetService<TService>(Enum type) where TService : class, ITypedService;
    }
}
