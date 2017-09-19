using System;
using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public interface IGroupFeedService : IFeedService
    {
        IEnumerable<IFeedItem> GetFeed(IIntranetType type, Guid groupId);
        IEnumerable<IFeedItem> GetFeed(Guid groupId);

        IEnumerable<IFeedItem> GetFeed(IIntranetType type, IEnumerable<Guid> groupId);
        IEnumerable<IFeedItem> GetFeed(IEnumerable<Guid> groupId);
    }
}