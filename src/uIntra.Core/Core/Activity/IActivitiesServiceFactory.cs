using System;

namespace uIntra.Core.Activity
{
    public interface IActivitiesServiceFactory
    {
        TService GetService<TService>(Guid id) where TService : class;
        TService GetService<TService>(int activityTypeId) where TService : class;
    }
}
