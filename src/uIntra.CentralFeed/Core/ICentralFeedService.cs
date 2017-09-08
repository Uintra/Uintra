using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedService
    {
        IEnumerable<IFeedItem> GetFeed(IIntranetType type);

        IEnumerable<IFeedItem> GetFeed();        

        long GetFeedVersion(IEnumerable<IFeedItem> centralFeedItems);

        CentralFeedSettings GetSettings(IIntranetType type);

        IEnumerable<CentralFeedSettings> GetAllSettings();
    }
}
