using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed
{
    public class LatestActivitiesViewModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public IEnumerable<ICentralFeedItem> Items { get; set; }
    }
}
