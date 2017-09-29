using System;
using System.Collections.Generic;
using uIntra.Core.Activity;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public abstract class FeedLinkProvider
    {
        protected abstract IEnumerable<string> FeedActivitiesXPath { get; }

        private readonly IActivityPageHelperFactory _pageHelperFactory;
        private readonly IProfileLinkProvider _profileLinkProvider;

        protected FeedLinkProvider(IActivityPageHelperFactory pageHelperFactory, IProfileLinkProvider profileLinkProvider)
        {
            _pageHelperFactory = pageHelperFactory;
            _profileLinkProvider = profileLinkProvider;
        }

        protected virtual string GetProfileLink(Guid userId) => _profileLinkProvider.GetProfileLink(userId);

        protected IActivityPageHelper GetPageHelper(IIntranetType type) => _pageHelperFactory.GetHelper(type, FeedActivitiesXPath);
    }
}