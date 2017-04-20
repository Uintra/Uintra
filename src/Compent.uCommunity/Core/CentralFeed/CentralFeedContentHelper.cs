using System.Collections.Generic;
using uCommunity.CentralFeed.Core;
using uCommunity.CentralFeed.Core.Models;
using uCommunity.CentralFeed.Enums;
using Umbraco.Core.Models;

namespace Compent.uCommunity.Core.CentralFeed
{
    public class CentralFeedContentHelper: ICentralFeedContentHelper
    {
        public IPublishedContent GetOverviewPage()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CentralFeedTabModel> GetTabs(IPublishedContent currentPage)
        {
            throw new System.NotImplementedException();
        }

        public CentralFeedTypeEnum GetTabType(IPublishedContent content)
        {
            throw new System.NotImplementedException();
        }

        public bool IsCentralFeedPage(IPublishedContent currentPage)
        {
            throw new System.NotImplementedException();
        }
    }
}