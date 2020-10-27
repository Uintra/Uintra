using System;
using System.Collections.Generic;
using Uintra.Core.Feed.Models;
using Uintra.Core.Feed.Services;

namespace Uintra.Features.Groups.Services
{
    public interface IGroupFeedService : IFeedSettingsService
    {
        IEnumerable<IFeedItem> GetFeed(Enum type, Guid groupId);
        IEnumerable<IFeedItem> GetFeed(Guid groupId);
        IEnumerable<IFeedItem> GetFeed(Enum type, IEnumerable<Guid> groupId);
    }
}