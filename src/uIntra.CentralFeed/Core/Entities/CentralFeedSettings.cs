using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public class CentralFeedSettings
    {
        public CentralFeedTypeEnum Type { get; set; }

        public string Controller { get; set; }

        public IPublishedContent OverviewPage { get; set; }
        public IPublishedContent CreatePage { get; set; }

        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }
        public bool HasBulletinFilter { get; set; }
    }
}