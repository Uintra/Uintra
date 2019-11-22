using System;
using System.Collections.Generic;
using Uintra20.Features.CentralFeed.Models.Feed;

namespace Uintra20.Features.CentralFeed
{
    public interface ICentralFeedService : IFeedService
    {
        IEnumerable<IFeedItem> GetFeed(Enum type);
        IEnumerable<IFeedItem> GetFeed();
    }
}
