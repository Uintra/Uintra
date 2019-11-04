using System;

namespace Uintra20.Core.Activity.Helpers
{
    public interface IActivityTypeHelper
    {
        Enum GetActivityType(Guid activityId);
    }
}
