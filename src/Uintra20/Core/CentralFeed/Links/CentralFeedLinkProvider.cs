using Uintra20.Core.Activity;
using Uintra20.Core.Links;

namespace Uintra20.Core.CentralFeed
{
    public class CentralFeedLinkProvider : FeedLinkProvider, ICentralFeedLinkProvider
    {
        public CentralFeedLinkProvider(
            IActivityPageHelperFactory pageHelperFactory,
            IProfileLinkProvider profileLinkProvider)
            : base(pageHelperFactory, profileLinkProvider)
        {
        }

        public IActivityLinks GetLinks(ActivityTransferModel activity)
        {
            var helper = GetPageHelper(activity.Type);

            return new ActivityLinks
            {
                Feed = helper.GetFeedUrl(),
                Overview = helper.GetOverviewPageUrl(),
                Create = helper.GetCreatePageUrl(),
                Details = helper.GetDetailsPageUrl(activity.Id),
                Edit = helper.GetEditPageUrl(activity.Id),
                Owner = GetProfileLink(activity.OwnerId),
                DetailsNoId = helper.GetDetailsPageUrl()
            };
        }

        public IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model)
        {
            var helper = GetPageHelper(model.Type);

            return new ActivityCreateLinks
            {
                Feed = helper.GetFeedUrl(),
                Overview = helper.GetOverviewPageUrl(),
                Create = helper.GetCreatePageUrl(),
                Owner = GetProfileLink(model.OwnerId),
                DetailsNoId = helper.GetDetailsPageUrl()
            };
        }
    }
}