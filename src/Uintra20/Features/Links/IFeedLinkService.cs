using System;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Links
{
    public interface IFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType);
        IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId);
    }
}
