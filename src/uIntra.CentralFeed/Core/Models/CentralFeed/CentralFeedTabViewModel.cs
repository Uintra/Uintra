using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class ActivityFeedTabViewModel
    {
        public IIntranetType Type { get; set; }       
        public bool IsActive { get; set; }
        public IActivityCreateLinks Links { get; set; }
    }
}
