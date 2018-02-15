using System;

namespace Uintra.CentralFeed
{
    public class CentralFeedTypeModel
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public string CreateUrl { get; set; }
        public string TabUrl { get; set; }
    }
}