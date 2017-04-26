using System.Collections.Generic;
using System.Linq;
using uCommunity.CentralFeed.Entities;
using uCommunity.CentralFeed.Enums;
using uCommunity.Core.Activity;

namespace uCommunity.CentralFeed.Models
{
    public class CentralFeedListViewModel
    {
        public CentralFeedTypeEnum? Type { get; set; }
        public IEnumerable<ICentralFeedItem> Items { get; set; }
        public IEnumerable<CentralFeedSettings> Settings { get; set; }
        public long Version { get; set; }
        public bool BlockScrolling { get; set; }
        public bool ShowSubscribed { get; set; }

        public CentralFeedListViewModel()
        {
            Items = Enumerable.Empty<ICentralFeedItem>();
            Settings = Enumerable.Empty<CentralFeedSettings>();
        }
    }
}