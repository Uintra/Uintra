using System.Collections.Generic;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedContentService : IFeedContentService
    {
        IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage);
        void SaveFiltersState(FeedFiltersState stateModel);
        T GetFiltersState<T>();
        bool CentralFeedCookieExists();
    }

    public interface ICentralFeedHelper
    {
        bool IsCentralFeedPage(IPublishedContent page);
    }
}