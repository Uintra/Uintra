using System;
using Uintra.Core.Activity.Helpers;
using Uintra.Features.Links;
using Uintra.Features.Links.Models;

namespace Uintra.Core.Feed
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