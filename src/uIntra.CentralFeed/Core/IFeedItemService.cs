using System;
using System.Collections.Generic;

namespace uIntra.CentralFeed
{
    public interface IFeedItemService
    {
        Enum ActivityType { get; }
        FeedSettings GetFeedSettings();
        IEnumerable<IFeedItem> GetItems();
    }
}