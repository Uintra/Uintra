using System;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public interface IGroupFeedLinkService : IActivityLinkService
    {
        ActivityCreateLinks GetCreateLinks(IIntranetType activityType, Guid groupId);
    }
}
