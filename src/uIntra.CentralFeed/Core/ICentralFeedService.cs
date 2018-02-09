using System;
using System.Collections.Generic;

namespace Uintra.CentralFeed
{
    public interface ICentralFeedService : IFeedService
    {
        IEnumerable<IFeedItem> GetFeed(Enum type);
        IEnumerable<IFeedItem> GetFeed();
    }
}
