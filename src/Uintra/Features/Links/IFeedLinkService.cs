using System;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Links
{
    public interface IFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType);
        IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId);
    }
}
