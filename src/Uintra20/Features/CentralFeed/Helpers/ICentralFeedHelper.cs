using Uintra20.Core.Feed.Models;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.UintraPanels.LastActivities.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.CentralFeed.Helpers
{
    public interface ICentralFeedHelper
    {
        LoadableFeedItemModel GetFeedItems(LatestActivitiesPanelModel node);
        string AvailableActivityTypes();
        FeedListViewModel GetFeedListViewModel(FeedListModel model);
        bool IsCentralFeedPage(IPublishedContent page);
    }
}
