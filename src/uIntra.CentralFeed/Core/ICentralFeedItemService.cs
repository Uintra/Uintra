using System;
using System.Collections.Generic;
using uIntra.CentralFeed.Core.Entities;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed.Core
{
    public interface ICentralFeedItemService
    {
        IntranetActivityTypeEnum ActivityType { get; }

        CentralFeedSettings GetCentralFeedSettings();

        ICentralFeedItem GetItem(Guid activityId);

        IEnumerable<ICentralFeedItem> GetItems();
    }
}