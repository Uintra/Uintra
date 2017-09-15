using System;
using uIntra.CentralFeed;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public interface IGroupFeedLinkService
    {
        ActivityLinks GetLinks(IFeedItem item, Guid groupId);
        ActivityCreateLinks GetCreateLinks(IIntranetType type, Guid groupId);
    }
}