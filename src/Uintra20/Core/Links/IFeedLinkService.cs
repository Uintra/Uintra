using System;
using Uintra20.Core.Links.Models;

namespace Uintra20.Core.Links
{
    public interface IFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType);

        IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId);
    }
}
