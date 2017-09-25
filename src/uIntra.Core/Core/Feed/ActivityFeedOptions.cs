using uIntra.Core.Links;

namespace uIntra.Core.Feed
{
    public class ActivityFeedOptions
    {
        public IActivityLinks Links { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
