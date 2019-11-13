using System;
using System.Threading.Tasks;

namespace Uintra20.Core.Links
{
    public interface IFeedLinkService : IActivityLinkService
    {
        IActivityCreateLinks GetCreateLinks(Enum activityType);
        IActivityCreateLinks GetCreateLinks(Enum activityType, Guid groupId);

        Task<IActivityCreateLinks> GetCreateLinksAsync(Enum activityType);
        Task<IActivityCreateLinks> GetCreateLinksAsync(Enum activityType, Guid groupId);
    }
}
