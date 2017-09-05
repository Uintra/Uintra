using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class CentralFeedListViewModel
    {
        public IIntranetType Type { get; set; }
        public IEnumerable<IFeedItem> Items { get; set; }
        public IEnumerable<CentralFeedSettings> Settings { get; set; }
        public long Version { get; set; }
        public bool BlockScrolling { get; set; }
        public bool ShowSubscribed { get; set; }
        public bool ShowPinned { get; set; }
        public bool IncludeBulletin { get; set; }

        public CentralFeedListViewModel()
        {
            Items = Enumerable.Empty<IFeedItem>();
            Settings = Enumerable.Empty<CentralFeedSettings>();
        }
    }
}