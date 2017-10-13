using System.Collections.Generic;
using uIntra.CentralFeed.Providers;
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
        bool IsCentralFeedPage(IPublishedContent currentPage);
    }

    public class CentralFeedHelper : ICentralFeedHelper
    {
        private readonly ICentralFeedContentProvider _contentProvider;

        public CentralFeedHelper(ICentralFeedContentProvider contentProvider)
        {
            _contentProvider = contentProvider;
        }

        public bool IsCentralFeedPage(IPublishedContent currentPage)
        {
            return _contentProvider.GetOverviewPage().Id == currentPage.Id || GetContents().Any(c => c.IsAncestorOrSelf(currentPage));
        }
    }
}