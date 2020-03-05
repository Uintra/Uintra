using System;
using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;

namespace Uintra20.Features.CentralFeed.Services
{
    public interface ICentralFeedService : IFeedSettingsService
    {
        IEnumerable<IFeedItem> GetFeed(Enum type);
        IEnumerable<IFeedItem> GetFeed();
    }
}
