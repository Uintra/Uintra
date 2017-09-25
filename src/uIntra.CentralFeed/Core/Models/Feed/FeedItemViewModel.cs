using uIntra.Core.Feed;

namespace uIntra.CentralFeed
{
    public class FeedItemViewModel
    {
        public IFeedItem Activity { get; set; }
        public string ControllerName { get; set; }
        public ActivityFeedOptions Options { get; set; }
    }
}