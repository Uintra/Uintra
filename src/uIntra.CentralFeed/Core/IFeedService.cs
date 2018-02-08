using System;
using System.Collections.Generic;

namespace uIntra.CentralFeed
{
    public interface IFeedService
    {
        long GetFeedVersion(IEnumerable<IFeedItem> centralFeedItems);
        FeedSettings GetSettings(Enum type);
        IEnumerable<FeedSettings> GetAllSettings();
    }
}