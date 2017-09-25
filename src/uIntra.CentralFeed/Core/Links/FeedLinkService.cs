using System.Collections.Generic;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public abstract class FeedLinkService
    {
        protected abstract IEnumerable<string> FeedActivitiesXPath { get; }

        private readonly IActivityPageHelperFactory _pageHelperFactory;

        protected FeedLinkService(IActivityPageHelperFactory pageHelperFactory)
        {
            _pageHelperFactory = pageHelperFactory;
        }

        protected IActivityPageHelper GetPageHelper(IIntranetType type) => _pageHelperFactory.GetHelper(type, FeedActivitiesXPath);
    }
}