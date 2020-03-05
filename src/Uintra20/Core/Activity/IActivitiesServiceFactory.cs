using System;
using System.Collections.Generic;

namespace Uintra20.Core.Activity
{
    public interface IActivitiesServiceFactory
    {
        TService GetService<TService>(Guid activityId) where TService : class, ITypedService;
        TService GetService<TService>(Enum type) where TService : class, ITypedService;
        IEnumerable<TService> GetServices<TService>() where TService : class, ITypedService;

    }
}
