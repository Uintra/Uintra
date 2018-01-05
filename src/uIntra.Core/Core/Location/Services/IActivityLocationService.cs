using System;
using uIntra.Core.Activity;

namespace uIntra.Core.Location
{
    public interface IActivityLocationService
    {
        ActivityLocation Get(Guid activityId);

        void Set(Guid activityId, ActivityLocation location);
    }
}
