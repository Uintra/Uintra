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

        IEnumerable<CentralFeedTabModel> GetTabs(IPublishedContent currentPage);

        void SaveFiltersState(CentralFeedFiltersStateModel stateModel);

        T GetFiltersState<T>();

        bool CentralFeedCookieExists();
    }
}