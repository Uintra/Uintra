using Uintra20.Features.CentralFeed.Models.GroupFeed;

namespace Uintra20.Core.Feed
{
    public class ActivityFeedOptionsWithGroups : ActivityFeedOptions
    {
        public GroupInfo? GroupInfo { get; set; }
    }
}