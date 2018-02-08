using System;

namespace uIntra.Core.Activity
{
    public interface IActivityTypeHelper
    {
        Enum GetActivityType(Guid activityId);
    }
}