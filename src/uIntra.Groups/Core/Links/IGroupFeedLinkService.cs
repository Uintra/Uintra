using System;
using Uintra.Core.Links;

namespace Uintra.Groups
{
    public interface IGroupFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId);
    }
}
