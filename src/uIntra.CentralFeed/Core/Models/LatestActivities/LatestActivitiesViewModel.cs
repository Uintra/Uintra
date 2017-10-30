using System.Collections.Generic;

namespace uIntra.CentralFeed
{
    public class LatestActivitiesViewModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public IEnumerable<FeedItemViewModel> Feed { get; set; }
        public ActivityFeedTabViewModel Tab { get; set; }
        public bool ShowSeeAllButton { get; set; }
    }
}
