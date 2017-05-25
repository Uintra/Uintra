using System.Collections.Generic;
using uIntra.CentralFeed.Core.Entities;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed.Core
{
    public interface ICentralFeedService
    {
        IEnumerable<ICentralFeedItem> GetFeed(IntranetActivityTypeEnum type);

        IEnumerable<ICentralFeedItem> GetFeed();        

        long GetFeedVersion(IEnumerable<ICentralFeedItem> centralFeedItems);

        CentralFeedSettings GetSettings(IntranetActivityTypeEnum type);

        IEnumerable<CentralFeedSettings> GetAllSettings();

        bool IsPinActual(ICentralFeedItem item);
    }
}
