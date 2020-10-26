using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Settings;

namespace Uintra.Core.Feed.Services
{
    public interface IFeedItemService
    {
        Enum Type { get; }
        FeedSettings GetFeedSettings();
        IEnumerable<IFeedItem> GetItems();
        IEnumerable<IFeedItem> GetGroupItems(Guid groupId);
        Task<IEnumerable<IFeedItem>> GetItemsAsync();
    }
}