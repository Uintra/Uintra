using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedItemService : IFeedItemService
    {
        IFeedItem GetItem(Guid activityId);

        IEnumerable<IFeedItem> GetItems();
    }

    public interface IGroupFeedItemService : IFeedItemService
    {
        IFeedItem GetItem(Guid activityId, Guid groupId);

        IEnumerable<IFeedItem> GetItems(Guid groupId);
    }

    public interface IFeedItemService
    {
        IIntranetType ActivityType { get; }

        FeedSettings GetCentralFeedSettings();
    }
}