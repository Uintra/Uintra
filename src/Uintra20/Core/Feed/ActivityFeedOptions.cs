using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Feed
{
    public class ActivityFeedOptions
    {
        public IActivityLinks Links { get; set; }
        public bool IsReadOnly { get; set; }
    }
}