using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedService
    {
        IEnumerable<ICentralFeedItem> GetFeed(IntranetActivityTypeEnum type);

        IEnumerable<ICentralFeedItem> GetFeed();        

        long GetFeedVersion(IEnumerable<ICentralFeedItem> centralFeedItems);

        CentralFeedSettings GetSettings(CentralFeedTypeEnum type);

        IEnumerable<CentralFeedSettings> GetAllSettings();

        bool IsPinActual(ICentralFeedItem item);
    }
}
