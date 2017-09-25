using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class FeedTabSettings
    {
        public IIntranetType Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }
    }
}