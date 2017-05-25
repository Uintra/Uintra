using uIntra.CentralFeed.Core.Enums;

namespace uIntra.CentralFeed.Core.Models
{
    public class CentralFeedTabViewModel
    {
        public CentralFeedTypeEnum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public string CreateUrl { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
    }
}
