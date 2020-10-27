using System.Collections.Generic;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Settings;

namespace Uintra.Core.Feed.Services
{
    public interface IFeedFilterService
    {
        IEnumerable<IFeedItem> ApplyFilters(IEnumerable<IFeedItem> items, FeedFilterStateModel filterState, FeedSettings settings);

        IEnumerable<IFeedItem> ApplyAdditionalFilters(IEnumerable<IFeedItem> feedItems);
    }
}
