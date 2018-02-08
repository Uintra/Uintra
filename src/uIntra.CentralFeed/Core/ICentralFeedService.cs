using System;
using System.Collections.Generic;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedService : IFeedService
    {
        IEnumerable<IFeedItem> GetFeed(Enum type);
        IEnumerable<IFeedItem> GetFeed();
    }
}
