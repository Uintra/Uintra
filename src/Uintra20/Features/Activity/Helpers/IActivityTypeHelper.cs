using System;

namespace Uintra20.Features.Activity.Helpers
{
    public interface IActivityTypeHelper
    {
        Enum GetActivityType(Guid activityId);
    }
}
