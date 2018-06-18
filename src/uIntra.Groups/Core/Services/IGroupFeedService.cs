using System;
using System.Collections.Generic;
using Uintra.CentralFeed;

namespace Uintra.Groups
{
    public interface IGroupFeedService : IFeedService
    {
        IEnumerable<IFeedItem> GetFeed(Enum type, Guid groupId);
        IEnumerable<IFeedItem> GetFeed(Guid groupId);

        IEnumerable<IFeedItem> GetFeed(Enum type, IEnumerable<Guid> groupId);
        IEnumerable<IFeedItem> GetFeed(IEnumerable<Guid> groupId);
    }
}