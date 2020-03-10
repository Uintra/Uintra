using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.CentralFeed.Commands;

namespace Uintra20.Features.CentralFeed.Builders
{
    public interface IActivityTabsBuilder
    {
        IEnumerable<ActivityFeedTabViewModel> Build(CentralFeedFilterCommand command);
        ActivityTabsBuilder BuildSocialTab();
        ActivityTabsBuilder BuildNewsTab();
        ActivityTabsBuilder BuildEventsTab();
    }
}