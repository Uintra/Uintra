using System;
using Uintra.Core.Links;

namespace Uintra.CentralFeed
{
    public class ActivityFeedTabViewModel
    {
        public Enum Type { get; set; }       
        public bool IsActive { get; set; }
        public IActivityCreateLinks Links { get; set; }
    }
}
