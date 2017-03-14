﻿namespace uCommunity.Core.Activity
{
    public interface IActivitiesServiceFactory
    {
        IIntranetActivityItemServiceBase GetService(IntranetActivityTypeEnum type);
    }
}
