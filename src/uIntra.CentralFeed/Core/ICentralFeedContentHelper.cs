using System.Collections.Generic;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedContentHelper
    {
        IPublishedContent GetOverviewPage();

        bool IsCentralFeedPage(IPublishedContent currentPage);

        CentralFeedTypeEnum GetTabType(IPublishedContent content);

        IEnumerable<CentralFeedTabModel> GetTabs(IPublishedContent currentPage);
    }
}
