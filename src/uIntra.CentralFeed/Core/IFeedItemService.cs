using System;
using System.Collections.Generic;

namespace Uintra.CentralFeed
{
    public interface IFeedItemService
    {
        Enum Type { get; }
        FeedSettings GetFeedSettings();
        IEnumerable<IFeedItem> GetItems();
    }
}