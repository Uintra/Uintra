using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class CentralFeedListViewModel
    {
        public IIntranetType Type { get; set; }
        public IEnumerable<IFeedItem> Items { get; set; } = Enumerable.Empty<IFeedItem>();
        public IEnumerable<CentralFeedSettings> Settings { get; set; } = Enumerable.Empty<CentralFeedSettings>();
        public long Version { get; set; }
        public bool BlockScrolling { get; set; }

        public FeedFilterStateViewModel FilterState { get; set; }
    }

    /// TODO : Move into separate file
    public class FeedFilterStateViewModel
    {
        public bool ShowSubscribed { get; set; }
        public bool ShowPinned { get; set; }
        public bool IncludeBulletin { get; set; }
    }
}