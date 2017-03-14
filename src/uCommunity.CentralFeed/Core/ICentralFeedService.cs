using System.Collections.Generic;
using uCommunity.CentralFeed.Entities;
using uCommunity.Core.Activity;

namespace uCommunity.CentralFeed
{
    public interface ICentralFeedService
    {
        IEnumerable<ICentralFeedItem> GetFeed(IntranetActivityTypeEnum type);

        IEnumerable<ICentralFeedItem> GetFeed();        

        long GetFeedVersion(IEnumerable<ICentralFeedItem> centralFeedItems);

        CentralFeedSettings GetSettings(IntranetActivityTypeEnum type);
    }
}
