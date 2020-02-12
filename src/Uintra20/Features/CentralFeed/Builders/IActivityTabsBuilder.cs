using System.Collections.Generic;
using Uintra20.Core.Feed.Models;

namespace Uintra20.Features.CentralFeed.Builders
{
    public interface IActivityTabsBuilder
    {
        IEnumerable<ActivityFeedTabViewModel> Build();
        ActivityTabsBuilder BuildSocialTab();
        ActivityTabsBuilder BuildNewsTab();
        ActivityTabsBuilder BuildEventsTab();
    }
}