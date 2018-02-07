using System;

namespace uIntra.CentralFeed
{
    public class FeedTabSettings
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }
    }
}