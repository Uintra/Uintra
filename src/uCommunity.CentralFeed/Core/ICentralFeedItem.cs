using System;
using uCommunity.Core.Activity;

namespace uCommunity.CentralFeed
{
    public interface ICentralFeedItem
    {
        Guid Id { get; set; }

        IntranetActivityTypeEnum Type { get; set; }

        DateTime PublishDate { get; }

        DateTime ModifyDate { get; }
    }
}