using System;

namespace uIntra.Core.Location
{
    public interface IActivityLocationService
    {
        ActivityLocation Get(Guid activityId);

        void Set(Guid activityId, ActivityLocation location);

        void DeleteForActivity(Guid activityId);
    }
}
