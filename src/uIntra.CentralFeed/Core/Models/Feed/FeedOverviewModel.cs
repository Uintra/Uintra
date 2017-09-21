using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class FeedOverviewModel
    {
        public IEnumerable<ActivityFeedTabViewModel> Tabs { get; set; }
        public IEnumerable<ActivityFeedTabViewModel> TabsWithCreateUrl { get; set; }
        public IIntranetType CurrentType { get; set; }
        public bool IsFiltersOpened { get; set; }
    }
}