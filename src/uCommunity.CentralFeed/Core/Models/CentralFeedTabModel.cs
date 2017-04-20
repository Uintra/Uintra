using uCommunity.CentralFeed.Enums;
using Umbraco.Core.Models;

namespace uCommunity.CentralFeed.Models
{
    public class CentralFeedTabModel
    {
        public IPublishedContent Content { get; set; }
        public CentralFeedTypeEnum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public string CreateUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
