using System;

namespace Uintra20.Features.CentralFeed.Models
{
    public class CentralFeedTypeModel
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public string CreateUrl { get; set; }
        public string TabUrl { get; set; }
    }
}