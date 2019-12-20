using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.UintraPanels.LastActivities.Models;

namespace Uintra20.Features.UintraPanels.LastActivities.Helpers
{
    public interface ILatestActivitiesHelper
    {
        (bool isShowMore, IEnumerable<FeedItemViewModel> feedItems) GetFeedItems(LatestActivitiesPanelModel node);
        string AvailableActivityTypes();
        FeedListViewModel GetFeedListViewModel(FeedListModel model);
    }
}
