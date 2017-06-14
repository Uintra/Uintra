using System;
using uIntra.Core.Activity;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedItem
    {
        Guid Id { get; }

        IntranetActivityTypeEnum Type { get; }

        DateTime PublishDate { get; }

        DateTime ModifyDate { get; }

        bool IsPinActual { get; }
    }
}