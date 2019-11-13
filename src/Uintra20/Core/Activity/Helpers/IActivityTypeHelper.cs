using System;

namespace Uintra20.Core.Activity
{
    public interface IActivityTypeHelper
    {
        Enum GetActivityType(Guid activityId);
    }
}
