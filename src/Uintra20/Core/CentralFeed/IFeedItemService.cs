using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.CentralFeed;

namespace Uintra20.Core.CentralFeed
{
    public interface IFeedItemService
    {
        Enum Type { get; }
        FeedSettings GetFeedSettings();
        IEnumerable<IFeedItem> GetItems();

        Task<IEnumerable<IFeedItem>> GetItemsAsync();
    }
}