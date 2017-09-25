using System;

namespace uIntra.Core.Links
{
    public interface IActivityLinkService
    {
        IActivityLinks GetLinks(Guid activityId);
    }
}