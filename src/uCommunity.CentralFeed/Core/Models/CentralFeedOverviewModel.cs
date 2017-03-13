using System.Collections.Generic;
using System.Linq;

namespace uCommunity.CentralFeed.Models
{
    public class CentralFeedOverviewModel
    {
        public IEnumerable<CentralFeedTypeModel> Types { get; set; }
        public CentralFeedOverviewModel()
        {
            Types = Enumerable.Empty<CentralFeedTypeModel>();
        }
    }
}