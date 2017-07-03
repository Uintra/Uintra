using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public class CentralFeedTabModel
    {
        public IPublishedContent Content { get; set; }
        public IIntranetType Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }        
        public string CreateUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
