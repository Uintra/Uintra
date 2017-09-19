using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class FeedTabViewModel
    {
        public IIntranetType Type { get; set; }       
        public bool IsActive { get; set; }
        public ActivityCreateLinks Links { get; set; }
    }
}
