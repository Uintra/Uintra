using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed.Web;

namespace uIntra.CentralFeed
{
    public class LatestActivitiesViewModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public IEnumerable<CentralFeedSettings> Settings { get; set; } = Enumerable.Empty<CentralFeedSettings>();
        public IEnumerable<FeedItemViewModel> Feed { get; set; }
        public CentralFeedTabViewModel Tab { get; set; }
    }
}
