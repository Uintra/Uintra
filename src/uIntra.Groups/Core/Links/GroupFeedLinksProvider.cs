using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Links;

namespace uIntra.Groups 
{
    public class GroupFeedLinksProvider : FeedLinkProvider, IGroupFeedLinksProvider
    {

        protected override IEnumerable<string> FeedActivitiesXPath => new[]
        {
            _aliasProvider.GetHomePage(),
            _aliasProvider.GetGroupOverviewPage(),
            _aliasProvider.GetGroupRoomPage()
        };

        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public GroupFeedLinksProvider(
            IActivityPageHelperFactory pageHelperFactory,
            IProfileLinkProvider profileLinkProvider,
            IDocumentTypeAliasProvider aliasProvider) : base(pageHelperFactory, profileLinkProvider)
        {
            _aliasProvider = aliasProvider;
        }

        public IActivityLinks GetLinks(GroupActivityTransferModel activity)
        {
            var helper = GetPageHelper(activity.Type);

            return new ActivityLinks()
            {
                Overview = helper.GetOverviewPageUrl().AddGroupId(activity.GroupId),
                Create = helper.GetCreatePageUrl()?.AddGroupId(activity.GroupId),
                Details = helper.GetDetailsPageUrl().AddIdParameter(activity.Id).AddGroupId(activity.GroupId),
                Edit = helper.GetEditPageUrl().AddIdParameter(activity.Id).AddGroupId(activity.GroupId),
                Creator = GetProfileLink(activity.CreatorId),
                DetailsNoId = helper.GetDetailsPageUrl().AddGroupId(activity.GroupId)
            };
        }

        public IActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model)
        {
            IActivityPageHelper helper = GetPageHelper(model.Type);

            return new ActivityCreateLinks()
            {
                Overview = helper.GetOverviewPageUrl().AddGroupId(model.GroupId),
                Create = helper.GetCreatePageUrl()?.AddGroupId(model.GroupId),
                Creator = GetProfileLink(model.CreatorId),
                DetailsNoId = helper.GetDetailsPageUrl().AddGroupId(model.GroupId)
            };
        }
    }
}
