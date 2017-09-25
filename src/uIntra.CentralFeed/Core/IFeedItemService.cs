using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface IFeedItemService
    {
        IIntranetType ActivityType { get; }
        FeedSettings GetFeedSettings();
        IEnumerable<IFeedItem> GetItems();
    }
}