using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class FeedOverviewModel
    {
        public IEnumerable<CentralFeedTabViewModel> Tabs { get; set; }
        public IIntranetType CurrentType { get; set; }
        public bool IsFiltersOpened { get; set; }
    }
}