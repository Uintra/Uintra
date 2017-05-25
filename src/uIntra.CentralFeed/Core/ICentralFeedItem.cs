using System;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedItem
    {
        Guid Id { get; set; }

        IntranetActivityTypeEnum Type { get; set; }

        DateTime PublishDate { get; }

        DateTime ModifyDate { get; }

        DateTime? EndPinDate { get; }

        bool IsPinned { get; }
    }
}