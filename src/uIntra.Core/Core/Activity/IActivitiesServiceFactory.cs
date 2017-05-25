﻿using System;

namespace uCommunity.Core.Activity
{
    public interface IActivitiesServiceFactory
    {
        TService GetService<TService>(Guid id) where TService : class;
        TService GetServiceSafe<TService>(Guid id) where TService : class;
        TService GetService<TService>(IntranetActivityTypeEnum type) where TService : class;
        TService GetServiceSafe<TService>(IntranetActivityTypeEnum type) where TService : class;
    }
}
