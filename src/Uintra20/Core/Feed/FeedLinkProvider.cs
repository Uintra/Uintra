using System;
using Uintra20.Core.Activity.Helpers;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Feed
{
    public abstract class FeedLinkProvider
    {
        private readonly IProfileLinkProvider _profileLinkProvider;
        protected readonly IActivityPageHelper _activityPageHelper;

        protected FeedLinkProvider(
            IActivityPageHelper activityPageHelper,
            IProfileLinkProvider profileLinkProvider)
        {
            _activityPageHelper = activityPageHelper;
            _profileLinkProvider = profileLinkProvider;
        }

        protected UintraLinkModel  GetProfileLink(Guid userId) => _profileLinkProvider.GetProfileLink(userId);
    }
}