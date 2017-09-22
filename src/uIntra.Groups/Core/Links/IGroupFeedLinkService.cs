using System;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public interface IGroupFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(IIntranetType activityType, Guid groupId);
    }
}
