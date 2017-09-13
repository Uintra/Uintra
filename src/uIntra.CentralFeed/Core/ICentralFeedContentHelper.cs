using System.Collections.Generic;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedContentHelper
    {
        IPublishedContent GetOverviewPage();

        bool IsCentralFeedPage(IPublishedContent currentPage);

        IIntranetType GetTabType(IPublishedContent content);

        IEnumerable<FeedTabModel> GetTabs(IPublishedContent currentPage);

        void SaveFiltersState(FeedFiltersState stateModel);

        T GetFiltersState<T>();

        bool CentralFeedCookieExists();
    }
}