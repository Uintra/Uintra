using System;
using System.Collections.Generic;

namespace Uintra20.Core.Feed.Models
{
    public class FeedOverviewModel
    {
        public IEnumerable<ActivityFeedTabViewModel> Tabs { get; set; }
        public bool IsFiltersOpened { get; set; }
    }
}