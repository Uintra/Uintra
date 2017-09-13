using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public class FeedItemViewModel
    {
        public IFeedItem Item { get; set; }
        public string ControllerName { get; set; }
        public ActivityLinks Links { get; set; }
    }
}