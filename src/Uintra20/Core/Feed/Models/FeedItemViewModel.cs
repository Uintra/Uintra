using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Activity.Models;

namespace Uintra20.Core.Feed.Models
{
    public class FeedItemViewModel
    {
        public IntranetActivityViewModelBase Activity { get; set; }
        public string ControllerName { get; set; }
        public ActivityFeedOptions Options { get; set; }
    }
}