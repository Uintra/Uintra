using System;
using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType);
    }
}
