using uIntra.Core.Activity;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
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