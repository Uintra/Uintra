using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public class FeedOverviewModel
    {
        public IEnumerable<FeedTabViewModel> Tabs { get; set; }
        public IEnumerable<FeedTabViewModel> TabsWithCreateUrl { get; set; }
        public IIntranetType CurrentType { get; set; }
    }
}