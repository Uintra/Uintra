using System;
using Uintra.Core.Links;

namespace Uintra.CentralFeed
{
    public interface IFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType);

        IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId);
    }
}
