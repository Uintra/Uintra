using System;
using uIntra.Core.Links;

namespace uIntra.Groups
{
    public interface IGroupFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId);
    }
}
