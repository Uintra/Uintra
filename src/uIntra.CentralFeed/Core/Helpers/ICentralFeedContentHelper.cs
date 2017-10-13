using System.Collections.Generic;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedContentHelper : IFeedContentHelper
    {
        bool IsCentralFeedPage(IPublishedContent currentPage);

        IIntranetType GetFeedTabType(IPublishedContent content);



        IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage);

        void SaveFiltersState(FeedFiltersState stateModel);

        T GetFiltersState<T>();

        bool CentralFeedCookieExists();
    }
}