using System;

namespace Uintra.Core.Activity
{
    public interface IActivityTypeHelper
    {
        Enum GetActivityType(Guid activityId);
    }
}