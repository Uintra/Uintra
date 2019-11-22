using System;
using System.Collections.Generic;
using Uintra20.Features.CentralFeed.Entities;
using Uintra20.Features.CentralFeed.Models.Feed;

namespace Uintra20.Features.CentralFeed
{
    public interface IFeedService
    {
        long GetFeedVersion(IEnumerable<IFeedItem> centralFeedItems);
        FeedSettings GetSettings(Enum type);
        IEnumerable<FeedSettings> GetAllSettings();
    }
}