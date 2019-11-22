using System.Collections.Generic;
using Uintra20.Features.CentralFeed.Entities;
using Uintra20.Features.CentralFeed.Models.Feed;

namespace Uintra20.Features.CentralFeed
{
    public interface IFeedFilterService
    {
        IEnumerable<IFeedItem> ApplyFilters(IEnumerable<IFeedItem> items, FeedFilterStateModel filterState, FeedSettings settings);

        IEnumerable<IFeedItem> ApplyAdditionalFilters(IEnumerable<IFeedItem> feedItems);
    }
}
