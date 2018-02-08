﻿using System;

namespace uIntra.CentralFeed
{
    public interface IFeedItem
    {
        Guid Id { get; }

        Guid OwnerId { get; set; }

        Enum Type { get; }

        DateTime PublishDate { get; }

        DateTime ModifyDate { get; }

        bool IsPinned { get; }

        bool IsPinActual { get; }
    }
}