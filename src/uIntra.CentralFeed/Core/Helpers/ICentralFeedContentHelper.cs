using System.Collections.Generic;
using uIntra.Core.TypeProviders;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedContentService : IFeedContentService
    {
        bool IsCentralFeedPage(IPublishedContent currentPage);




        IEnumerable<ActivityFeedTabModel> GetTabs(IPublishedContent currentPage);

        void SaveFiltersState(FeedFiltersState stateModel);

        T GetFiltersState<T>();

        bool CentralFeedCookieExists();
    }
}