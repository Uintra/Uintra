using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedItemService
    {
        IIntranetType ActivityType { get; }

        CentralFeedSettings GetCentralFeedSettings();

        ICentralFeedItem GetItem(Guid activityId);

        IEnumerable<ICentralFeedItem> GetItems();
    }
}