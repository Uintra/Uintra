using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public class CentralFeedTabModel
    {
        public IPublishedContent Content { get; set; }
        public CentralFeedTypeEnum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }
        public bool HasBulletinFilter { get; set; }
        public string CreateUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
