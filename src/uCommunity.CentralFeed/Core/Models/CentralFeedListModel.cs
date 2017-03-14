using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Activity;

namespace uCommunity.CentralFeed.Models
{
    public class CentralFeedListModel
    {
        public IntranetActivityTypeEnum? Type { get; set; }
        public IEnumerable<ICentralFeedItem> Items { get; set; }
        public long Version { get; set; }
        public bool BlockScrolling { get; set; }

        public CentralFeedListModel()
        {
            Items = Enumerable.Empty<ICentralFeedItem>();
        }
    }
}