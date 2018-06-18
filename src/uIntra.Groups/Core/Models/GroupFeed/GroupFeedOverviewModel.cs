using System;
using uIntra.CentralFeed;

namespace uIntra.Groups
{
    public class GroupFeedOverviewModel : FeedOverviewModel
    {
        public Guid GroupId { get; set; }
        public bool IsGroupMember { get; set; }
    }
}
