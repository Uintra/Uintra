namespace Uintra20.Core.Feed.Models
{
    public class FeedItemViewModel
    {
        public IFeedItem Activity { get; set; }
        public string ControllerName { get; set; }
        public ActivityFeedOptions Options { get; set; }
    }
}