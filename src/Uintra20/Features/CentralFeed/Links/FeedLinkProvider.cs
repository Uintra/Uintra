using System;
using Uintra20.Features.Activity.Factories;
using Uintra20.Features.Activity.Helpers;
using Uintra20.Features.Links;

namespace Uintra20.Features.CentralFeed.Links
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