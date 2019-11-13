using Uintra20.Core.Links;

namespace Uintra20.Core.Feed
{
    public class ActivityFeedOptions
    {
        public IActivityLinks Links { get; set; }
        public bool IsReadOnly { get; set; }
    }
}