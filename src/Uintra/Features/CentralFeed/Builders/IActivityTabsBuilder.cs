using System.Collections.Generic;
using Uintra.Core.Feed.Models;
using Uintra.Features.CentralFeed.Commands;

namespace Uintra.Features.CentralFeed.Builders
{
    public interface IActivityTabsBuilder
    {
        IEnumerable<ActivityFeedTabViewModel> Build(CentralFeedFilterCommand command);
        ActivityTabsBuilder BuildSocialTab();
        ActivityTabsBuilder BuildNewsTab();
        ActivityTabsBuilder BuildEventsTab();
    }
}