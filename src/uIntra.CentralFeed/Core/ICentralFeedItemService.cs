using System;
using System.Collections.Generic;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedItemService
    {
        IntranetActivityTypeEnum ActivityType { get; }

        CentralFeedSettings GetCentralFeedSettings();

        ICentralFeedItem GetItem(Guid activityId);

        IEnumerable<ICentralFeedItem> GetItems();
    }
}