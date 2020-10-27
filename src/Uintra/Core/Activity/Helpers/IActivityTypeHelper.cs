using System;

namespace Uintra.Core.Activity.Helpers
{
    public interface IActivityTypeHelper
    {
        Enum GetActivityType(Guid activityId);
    }
}
