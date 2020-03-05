using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Settings;

namespace Uintra20.Core.Feed.Services
{
    public interface IFeedItemService
    {
        Enum Type { get; }
        FeedSettings GetFeedSettings();
        IEnumerable<IFeedItem> GetItems();
        Task<IEnumerable<IFeedItem>> GetItemsAsync();
    }
}