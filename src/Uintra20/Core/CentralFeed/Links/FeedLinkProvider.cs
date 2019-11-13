using System;
using Uintra20.Core.Activity;
using Uintra20.Core.Links;

namespace Uintra20.Core.CentralFeed
{
    public abstract class FeedLinkProvider
    {
        private readonly IActivityPageHelperFactory _pageHelperFactory;
        private readonly IProfileLinkProvider _profileLinkProvider;

        protected FeedLinkProvider(IActivityPageHelperFactory pageHelperFactory, IProfileLinkProvider profileLinkProvider)
        {
            _pageHelperFactory = pageHelperFactory;
            _profileLinkProvider = profileLinkProvider;
        }

        protected virtual string GetProfileLink(Guid userId) => _profileLinkProvider.GetProfileLink(userId);

        protected IActivityPageHelper GetPageHelper(Enum type) => _pageHelperFactory.GetHelper(type);
    }
}