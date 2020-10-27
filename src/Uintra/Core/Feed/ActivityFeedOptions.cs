using Uintra.Features.Links.Models;

namespace Uintra.Core.Feed
{
    public class ActivityFeedOptions
    {
        public IActivityLinks Links { get; set; }
        public bool IsReadOnly { get; set; }
    }
}