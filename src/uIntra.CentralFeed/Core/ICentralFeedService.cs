using System.Collections.Generic;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedService
    {
        IEnumerable<ICentralFeedItem> GetFeed(IIntranetType type);

        IEnumerable<ICentralFeedItem> GetFeed();        

        long GetFeedVersion(IEnumerable<ICentralFeedItem> centralFeedItems);

        CentralFeedSettings GetSettings(IIntranetType type);

        IEnumerable<CentralFeedSettings> GetAllSettings();
    }
}
