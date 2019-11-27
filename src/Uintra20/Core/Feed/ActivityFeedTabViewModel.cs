using System;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Feed
{
    public class ActivityFeedTabViewModel
    {
        public Enum Type { get; set; }       
        public bool IsActive { get; set; }
        public IActivityCreateLinks Links { get; set; }
    }
}
