using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.CentralFeed.Entities;
using Uintra20.Features.CentralFeed.Models.Feed;

namespace Uintra20.Features.CentralFeed
{
    public interface IFeedItemService
    {
        Enum Type { get; }
        FeedSettings GetFeedSettings();
        IEnumerable<IFeedItem> GetItems();

        Task<IEnumerable<IFeedItem>> GetItemsAsync();
    }
}