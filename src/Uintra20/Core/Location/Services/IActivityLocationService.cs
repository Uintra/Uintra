using System;

namespace Uintra20.Core.Location.Services
{
    public interface IActivityLocationService
    {
        ActivityLocation Get(Guid activityId);

        void Set(Guid activityId, ActivityLocation location);

        void DeleteForActivity(Guid activityId);
    }
}
