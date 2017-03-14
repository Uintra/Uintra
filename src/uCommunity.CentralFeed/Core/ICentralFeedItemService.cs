using System;
using System.Collections.Generic;
using uCommunity.CentralFeed.Entities;
using uCommunity.Core.Activity;

namespace uCommunity.CentralFeed
{
    public interface ICentralFeedItemService
    {
        IntranetActivityTypeEnum ActivityType { get; }

        CentralFeedSettings GetCentralFeedSettings();

        ICentralFeedItem GetItem(Guid activityId);

        IEnumerable<ICentralFeedItem> GetItems();
    }
}