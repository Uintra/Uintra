using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed.Core.Models
{
    public class LatestActivitiesModel
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public IEnumerable<IIntranetActivity> Activities { get; set; }
    }
}
