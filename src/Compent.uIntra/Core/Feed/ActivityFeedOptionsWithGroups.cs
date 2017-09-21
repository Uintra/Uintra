using uIntra.CentralFeed;
using uIntra.Groups;

namespace Compent.uIntra.Core.Feed
{
    public class ActivityFeedOptionsWithGroups : ActivityFeedOptions
    {
        public GroupInfo? GroupInfo { get; set; }
    }
}