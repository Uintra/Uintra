using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public interface IActivityTypeHelper
    {
        IIntranetType GetType(Guid activityId);
    }
}