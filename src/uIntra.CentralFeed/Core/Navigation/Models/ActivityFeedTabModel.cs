using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class ActivityFeedTabModel : TabModelBase
    {
        public IIntranetType Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }        
        public IActivityCreateLinks Links { get; set; }
    }
}
