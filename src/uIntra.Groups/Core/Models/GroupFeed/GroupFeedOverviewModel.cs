using System;
using Uintra.CentralFeed;

namespace Uintra.Groups
{
    public class GroupFeedOverviewModel : FeedOverviewModel
    {
        public Guid GroupId { get; set; }
        public bool IsGroupMember { get; set; }
    }
}
