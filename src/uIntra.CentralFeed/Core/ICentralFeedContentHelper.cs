using System.Collections.Generic;
using uCommunity.CentralFeed.Enums;
using uCommunity.CentralFeed.Models;
using Umbraco.Core.Models;

namespace uCommunity.CentralFeed.Core
{
    public interface ICentralFeedContentHelper
    {
        IPublishedContent GetOverviewPage();

        bool IsCentralFeedPage(IPublishedContent currentPage);

        CentralFeedTypeEnum GetTabType(IPublishedContent content);

        IEnumerable<CentralFeedTabModel> GetTabs(IPublishedContent currentPage);
    }
}
