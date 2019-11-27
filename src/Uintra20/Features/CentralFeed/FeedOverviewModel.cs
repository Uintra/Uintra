using System;
using System.Collections.Generic;
using Uintra20.Core.Activity;
using Uintra20.Core.Feed;

namespace Uintra20.Features.CentralFeed
{
    public class FeedOverviewModel
    {
        public IEnumerable<ActivityFeedTabViewModel> Tabs { get; set; }
        public IEnumerable<ActivityFeedTabViewModel> TabsWithCreateUrl { get; set; }
        public Enum CurrentType { get; set; }
        public bool IsFiltersOpened { get; set; }
    }
}