using System;
using System.Collections.Generic;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Feed.Models
{
    public class ActivityFeedTabViewModel
    {
        public Enum Type { get; set; }       
        public bool IsActive { get; set; }
        public IActivityCreateLinks Links { get; set; }
        public string Title { get; set; }
        public Dictionary<string,string> Filters { get; set; } = new Dictionary<string, string>();
    }
}
