using System;
using Uintra.Features.Links.Models;

namespace Uintra.Core.Feed.Models
{
    public class ActivityFeedTabViewModel
    {
        public Enum Type { get; set; }
        public bool IsActive { get; set; }
        public IActivityCreateLinks Links { get; set; }
        public string Title { get; set; }
        public ActivityFeedTabFiltersViewModel[] Filters { get; set; } = new ActivityFeedTabFiltersViewModel[]{} ;
    }
}
