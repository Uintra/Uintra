using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed.Web;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class CentralFeedListViewModel
    {
        public IIntranetType Type { get; set; }
        public IEnumerable<FeedItemViewModel> Feed { get; set; } = Enumerable.Empty<FeedItemViewModel>();
        public IEnumerable<CentralFeedSettings> Settings { get; set; } = Enumerable.Empty<CentralFeedSettings>();
        public long Version { get; set; }
        public bool BlockScrolling { get; set; }
        public FeedFilterStateViewModel FilterState { get; set; }
    }
}