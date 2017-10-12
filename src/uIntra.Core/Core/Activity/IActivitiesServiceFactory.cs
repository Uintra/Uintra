using System;

namespace uIntra.Core.Activity
{
    public interface IActivitiesServiceFactory
    {
        TService GetService<TService>(Guid activityId) where TService : ITypedService;
        TService GetService<TService>(int typeId) where TService : ITypedService;
    }
}
