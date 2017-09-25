using System;
using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core.TypeProviders;

namespace uIntra.Groups
{
    public interface IGroupFeedService1
    {
        IEnumerable<IFeedItem> GetFeed(Guid groupId);
        IEnumerable<IFeedItem> GetFeed(IIntranetType type, Guid groupId);
    }
}