using Uintra20.Core.Feed.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.CentralFeed.Helpers
{
    public interface ICentralFeedHelper
    {
        string AvailableActivityTypes();
        FeedListViewModel GetFeedItems(FeedListModel model);
        bool IsCentralFeedPage(IPublishedContent page);
    }
}
