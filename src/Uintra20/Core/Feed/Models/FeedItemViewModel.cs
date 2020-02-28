using Uintra20.Core.Activity.Models;

namespace Uintra20.Core.Feed.Models
{
    public class FeedItemViewModel
    {
        public IntranetActivityPreviewModelBase Activity { get; set; }
        public ActivityFeedOptions Options { get; set; }
    }
}