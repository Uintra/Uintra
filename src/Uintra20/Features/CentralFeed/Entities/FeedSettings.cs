﻿using System;

namespace Uintra20.Features.CentralFeed.Entities
{
    public class FeedSettings
    {
        public Enum Type { get; set; }
        public string Controller { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }

        public bool ExcludeFromAvailableActivityTypes { get; set; }
        public bool ExcludeFromLatestActivities { get; set; }
    }
}