using System;
using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Settings;

namespace Uintra20.Core.Feed.Services
{
    public interface IFeedService
    {
        long GetFeedVersion(IEnumerable<IFeedItem> centralFeedItems);
        FeedSettings GetSettings(Enum type);
        IEnumerable<FeedSettings> GetAllSettings();
    }
}