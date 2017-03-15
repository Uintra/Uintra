﻿using System;

namespace uCommunity.Core.Activity
{
    public interface IActivitiesServiceFactory
    {
        IIntranetActivityItemServiceBase GetService(Guid id);
        IIntranetActivityItemServiceBase GetService(IntranetActivityTypeEnum type);
    }
}
