using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed
{
    public class LatestActivitiesModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public IEnumerable<IIntranetActivity> Activities { get; set; }
    }
}
