using System;
using System.Collections.Generic;
using Uintra20.CentralFeed;

namespace Uintra20.Core.CentralFeed
{
    public interface IFeedItemService
    {
        Enum Type { get; }
        FeedSettings GetFeedSettings();
        IEnumerable<IFeedItem> GetItems();
    }
}