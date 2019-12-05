using System;

namespace Uintra20.Core.Feed.Settings
{
    public class FeedTabSettings
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }
    }
}