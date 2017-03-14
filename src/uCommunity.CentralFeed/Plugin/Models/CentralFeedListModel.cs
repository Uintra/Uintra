using uCommunity.Core.Activity;

namespace uCommunity.CentralFeed.Models
{
    class CentralFeedListModel
    {
        public IntranetActivityTypeEnum? Type { get; set; }
        public bool? ShowSubscribed { get; set; }
        public long? Version { get; set; }
        public int Page { get; set; } = 1;
    }
}
