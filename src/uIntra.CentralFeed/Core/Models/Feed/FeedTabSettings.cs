using System;

namespace Uintra.CentralFeed
{
    public class FeedTabSettings
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }
    }
}