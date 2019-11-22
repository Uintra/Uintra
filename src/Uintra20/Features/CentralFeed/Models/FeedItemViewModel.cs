using Uintra20.Core.Feed;
using Uintra20.Features.CentralFeed.Models.Feed;

namespace Uintra20.Features.CentralFeed.Models
{
    public class FeedItemViewModel
    {
        public IFeedItem Activity { get; set; }
        public string ControllerName { get; set; }
        public ActivityFeedOptions Options { get; set; }
    }
}