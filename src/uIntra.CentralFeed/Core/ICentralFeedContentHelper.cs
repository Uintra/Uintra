using System.Collections.Generic;
using uIntra.CentralFeed.Core.Enums;
using uIntra.CentralFeed.Core.Models;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed.Core
{
    public interface ICentralFeedContentHelper
    {
        IPublishedContent GetOverviewPage();

        bool IsCentralFeedPage(IPublishedContent currentPage);

        CentralFeedTypeEnum GetTabType(IPublishedContent content);

        IEnumerable<CentralFeedTabModel> GetTabs(IPublishedContent currentPage);
    }
}
