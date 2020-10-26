using Uintra.Features.CentralFeed.Models.GroupFeed;

namespace Uintra.Core.Feed
{
    public class ActivityFeedOptionsWithGroups : ActivityFeedOptions
    {
        public GroupInfo? GroupInfo { get; set; }
    }
}