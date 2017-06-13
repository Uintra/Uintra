using System.Collections.Generic;

namespace uIntra.CentralFeed
{
    public class CentralFeedOverviewModel
    {
        public IEnumerable<CentralFeedTabViewModel> Tabs { get; set; }
        public CentralFeedTypeEnum CurrentType { get; set; }
    }
}