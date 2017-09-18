using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedItemService : IFeedItemService
    {
        IEnumerable<IFeedItem> GetItems();
    }

    public interface IGroupFeedItemService : IFeedItemService
    {
        IEnumerable<IFeedItem> GetItems(Guid groupId);
    }

    public interface IFeedItemService
    {
        IIntranetType ActivityType { get; }

        FeedSettings GetCentralFeedSettings();
    }
}