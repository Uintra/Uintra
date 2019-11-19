using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Feed
{
    public class ActivityFeedOptions
    {
        public IActivityLinks Links { get; set; }
        public bool IsReadOnly { get; set; }
    }
}