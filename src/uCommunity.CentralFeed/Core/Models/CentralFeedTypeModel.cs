using uCommunity.Core.Activity;

namespace uCommunity.CentralFeed.Models
{
    public class CentralFeedTypeModel
    {
        public IntranetActivityTypeEnum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public string CreateUrl { get; set; }
        public string TabUrl { get; set; }
    }
}