using Uintra.Core.Feed;

namespace Uintra.CentralFeed
{
    public class FeedItemViewModel
    {
        public IFeedItem Activity { get; set; }
        public string ControllerName { get; set; }
        public ActivityFeedOptions Options { get; set; }
    }
}