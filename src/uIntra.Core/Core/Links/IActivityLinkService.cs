using System;

namespace uIntra.Core.Links
{
    public interface IActivityLinkService
    {
        ActivityLinks GetLinks(Guid activityId);
    }
}