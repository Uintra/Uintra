using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed.Core.Models
{
    public class LatestActivitiesViewModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public IEnumerable<IIntranetActivity> Activities { get; set; }
    }
}
