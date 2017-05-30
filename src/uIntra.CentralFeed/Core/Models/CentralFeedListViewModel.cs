using System.Collections.Generic;
using System.Linq;

namespace uIntra.CentralFeed
{
    public class CentralFeedListViewModel
    {
        public CentralFeedTypeEnum? Type { get; set; }
        public IEnumerable<ICentralFeedItem> Items { get; set; }
        public IEnumerable<CentralFeedSettings> Settings { get; set; }
        public long Version { get; set; }
        public bool BlockScrolling { get; set; }
        public bool ShowSubscribed { get; set; }
        public bool ShowPinned { get; set; }
        public bool IncludeBulletin { get; set; }

        public CentralFeedListViewModel()
        {
            Items = Enumerable.Empty<ICentralFeedItem>();
            Settings = Enumerable.Empty<CentralFeedSettings>();
        }
    }
}