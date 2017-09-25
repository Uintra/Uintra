using System.Collections.Generic;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedContentHelper
    {
        IPublishedContent GetOverviewPage();

        bool IsCentralFeedPage(IPublishedContent currentPage);

        IIntranetType GetCentralFeedTabType(IPublishedContent content);

        IIntranetType GetCreateActivityType(IPublishedContent content);

        IIntranetType GetActivityTypeFromPlugin(IPublishedContent content, string gridPluginAlias);

        IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage);

        void SaveFiltersState(FeedFiltersState stateModel);

        T GetFiltersState<T>();

        bool CentralFeedCookieExists();
    }
}