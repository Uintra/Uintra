using System;
using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public class ActivityFeedTabViewModel
    {
        public Enum Type { get; set; }       
        public bool IsActive { get; set; }
        public IActivityCreateLinks Links { get; set; }
    }
}
