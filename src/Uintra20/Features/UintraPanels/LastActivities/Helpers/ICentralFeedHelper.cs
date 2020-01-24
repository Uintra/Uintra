using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Features.UintraPanels.LastActivities.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.UintraPanels.LastActivities.Helpers
{
    public interface ICentralFeedHelper
    {
        (bool isShowMore, IEnumerable<FeedItemViewModel> feedItems) GetFeedItems(LatestActivitiesPanelModel node);
        string AvailableActivityTypes();
        FeedListViewModel GetFeedListViewModel(FeedListModel model);
        bool IsCentralFeedPage(IPublishedContent page);
    }
}
