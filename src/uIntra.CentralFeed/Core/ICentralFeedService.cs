using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedService
    {
        IEnumerable<IFeedItem> GetFeed(IIntranetType type);

        IEnumerable<IFeedItem> GetFeed();        

        long GetFeedVersion(IEnumerable<IFeedItem> centralFeedItems);

        FeedSettings GetSettings(IIntranetType type);

        IEnumerable<FeedSettings> GetAllSettings();
    }
}
