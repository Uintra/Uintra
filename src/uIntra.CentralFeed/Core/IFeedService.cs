using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface IFeedService
    {
        long GetFeedVersion(IEnumerable<IFeedItem> centralFeedItems);
        FeedSettings GetSettings(IIntranetType type);
        IEnumerable<FeedSettings> GetAllSettings();
    }
}