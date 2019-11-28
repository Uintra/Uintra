using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Settings;

namespace Uintra20.Core.Feed.Services
{
    public interface IFeedFilterService
    {
        IEnumerable<IFeedItem> ApplyFilters(IEnumerable<IFeedItem> items, FeedFilterStateModel filterState, FeedSettings settings);

        IEnumerable<IFeedItem> ApplyAdditionalFilters(IEnumerable<IFeedItem> feedItems);
    }
}
