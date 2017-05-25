using uIntra.Core.Activity;
using Umbraco.Core.Models;

namespace uCommunity.CentralFeed.Entities
{
    public class CentralFeedSettings
    {
        public IntranetActivityTypeEnum Type { get; set; }

        public string Controller { get; set; }

        public IPublishedContent OverviewPage { get; set; }
        public IPublishedContent CreatePage { get; set; }

        public bool HasSubscribersFitler { get; set; }
    }
}