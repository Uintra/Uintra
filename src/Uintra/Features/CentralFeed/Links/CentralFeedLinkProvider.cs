using Uintra.Core.Activity.Helpers;
using Uintra.Core.Feed;
using Uintra.Features.CentralFeed.Models;
using Uintra.Features.Links;
using Uintra.Features.Links.Models;

namespace Uintra.Features.CentralFeed.Links
{
    public class CentralFeedLinkProvider : 
        FeedLinkProvider, 
        ICentralFeedLinkProvider
    {
        public CentralFeedLinkProvider(
            IActivityPageHelper activityPageHelper, 
            IProfileLinkProvider profileLinkProvider) 
            : base(activityPageHelper, profileLinkProvider)
        {
        }

        public IActivityLinks GetLinks(ActivityTransferModel activity)
        {
            var result =  new ActivityLinks
            {
                Feed = _activityPageHelper.GetFeedUrl(),
                Create = _activityPageHelper.GetCreatePageUrl(activity.Type),
                Details = _activityPageHelper.GetDetailsPageUrl(activity.Type, activity.Id),
                Edit = _activityPageHelper.GetEditPageUrl(activity.Type, activity.Id),
                Owner = GetProfileLink(activity.OwnerId),
            };

            return result;
        }

        public IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel activity)
        {
            return new ActivityCreateLinks
            {
                Feed = _activityPageHelper.GetFeedUrl(),
                Create = _activityPageHelper.GetCreatePageUrl(activity.Type),
                Owner = GetProfileLink(activity.OwnerId),
            };
        }

        
    }
}