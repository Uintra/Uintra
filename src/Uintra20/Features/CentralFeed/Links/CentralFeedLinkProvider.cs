using Uintra20.Core.Activity.Helpers;
using Uintra20.Core.Feed;
using Uintra20.Features.CentralFeed.Models;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.CentralFeed.Links
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
                Overview = null,//helper.GetOverviewPageUrl(),//TODO: Research overview pages
                Create = _activityPageHelper.GetCreatePageUrl(activity.Type),
                Details = _activityPageHelper.GetDetailsPageUrl(activity.Type, activity.Id),
                Edit = _activityPageHelper.GetEditPageUrl(activity.Type, activity.Id),
                Owner = GetProfileLink(activity.OwnerId),
                DetailsNoId = _activityPageHelper.GetDetailsPageUrl(activity.Type)
            };

            return result;
        }

        public IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel activity)
        {
            return new ActivityCreateLinks
            {
                Feed = _activityPageHelper.GetFeedUrl(),
                Overview = null,//helper.GetOverviewPageUrl(),//TODO: Research overview pages
                Create = _activityPageHelper.GetCreatePageUrl(activity.Type),
                Owner = GetProfileLink(activity.OwnerId),
                DetailsNoId = _activityPageHelper.GetDetailsPageUrl(activity.Type)
            };
        }

        
    }
}