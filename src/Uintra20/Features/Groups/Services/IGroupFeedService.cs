using System;
using System.Collections.Generic;
using Uintra20.Core.Feed.Models;
using Uintra20.Core.Feed.Services;

namespace Uintra20.Features.Groups.Services
{
    public interface IGroupFeedService : IFeedSettingsService
    {
        IEnumerable<IFeedItem> GetFeed(Enum type, Guid groupId);
        IEnumerable<IFeedItem> GetFeed(Guid groupId);
        IEnumerable<IFeedItem> GetFeed(Enum type, IEnumerable<Guid> groupId);
        IEnumerable<IFeedItem> GetFeed(IEnumerable<Guid> groupId);
    }
}