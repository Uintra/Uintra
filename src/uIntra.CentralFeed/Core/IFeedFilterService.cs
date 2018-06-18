using System.Collections.Generic;

namespace Uintra.CentralFeed
{
    public interface IFeedFilterService
    {
        IEnumerable<IFeedItem> ApplyFilters(IEnumerable<IFeedItem> items, FeedFilterStateModel filterState, FeedSettings settings);

        IEnumerable<IFeedItem> ApplyAdditionalFilters(IEnumerable<IFeedItem> feedItems);
    }
}
