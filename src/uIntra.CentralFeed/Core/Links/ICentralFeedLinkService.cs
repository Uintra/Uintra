using System;
using Uintra.Core.Links;

namespace Uintra.CentralFeed
{
    public interface ICentralFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType);
    }
}
