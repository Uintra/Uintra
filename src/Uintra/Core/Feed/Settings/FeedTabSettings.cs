using System;

namespace Uintra.Core.Feed.Settings
{
    public class FeedTabSettings
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }
    }
}