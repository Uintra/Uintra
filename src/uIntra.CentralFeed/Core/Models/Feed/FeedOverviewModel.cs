using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public abstract class FeedOverviewModel
    {
        public IEnumerable<CentralFeedTabViewModel> Tabs { get; set; }
        public IIntranetType CurrentType { get; set; }
        public bool IsFiltersOpened { get; set; }
    }
}