using System;
using System.Collections.Generic;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Services;

namespace Uintra.Features.CentralFeed.Services
{
    public interface ICentralFeedService : IFeedSettingsService
    {
        IEnumerable<IFeedItem> GetFeed(Enum type);
        IEnumerable<IFeedItem> GetFeed();
    }
}
