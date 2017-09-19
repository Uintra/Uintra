using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public class FeedTabModel
    {
        public IPublishedContent Content { get; set; }
        public IIntranetType Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }        
        public bool IsActive { get; set; }
        public ActivityCreateLinks Links { get; set; }
    }
}
