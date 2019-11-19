using System;
using System.Threading.Tasks;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Links
{
    public interface IFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType);
        IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId);

        Task<IActivityCreateLinks> GetCreateLinksAsync(Enum activityType);
        Task<IActivityCreateLinks> GetCreateLinksAsync(Enum activityType, Guid groupId);
    }
}
