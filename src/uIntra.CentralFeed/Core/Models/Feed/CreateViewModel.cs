using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public class CreateViewModel
    {
        public IActivityCreateLinks Links { get; set; }
        public FeedSettings Settings { get; set; }
    }
}