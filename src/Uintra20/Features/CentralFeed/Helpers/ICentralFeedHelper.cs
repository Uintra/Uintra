using System;
using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.CentralFeed.Helpers
{
    public interface ICentralFeedHelper
    {
        string AvailableActivityTypes();
        bool IsCentralFeedPage(IPublishedContent page);
        IEnumerable<IFeedItem> GetCentralFeedItems(Enum centralFeedType);
        IEnumerable<IFeedItem> GetGroupFeedItems(Enum type, Guid groupId);
        IEnumerable<IFeedItem> Sort(IEnumerable<IFeedItem> sortedItems, Enum type);
    }
}
