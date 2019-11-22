using System;

namespace Uintra20.Features.CentralFeed
{
    public class FeedTabSettings
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }
    }
}