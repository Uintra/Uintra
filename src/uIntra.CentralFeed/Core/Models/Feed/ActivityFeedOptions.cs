using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public class ActivityFeedOptions
    {
        public IActivityLinks Links { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
