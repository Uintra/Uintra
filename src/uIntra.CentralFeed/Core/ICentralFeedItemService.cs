using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedItemService
    {
        IIntranetType ActivityType { get; }

        FeedSettings GetCentralFeedSettings();

        IFeedItem GetItem(Guid activityId);

        IEnumerable<IFeedItem> GetItems();
    }

    public interface IGroupFeedItemService
    {
        IIntranetType ActivityType { get; }

        FeedSettings GetCentralFeedSettings();

        IFeedItem GetItem(Guid activityId);

        IEnumerable<IFeedItem> GetItems();
    }
}