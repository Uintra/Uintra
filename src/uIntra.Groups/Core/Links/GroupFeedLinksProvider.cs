using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.User;

namespace uIntra.Groups 
{
    public class GroupFeedLinksProvider : FeedLinkService, IGroupFeedLinksProvider
    {

        protected override IEnumerable<string> FeedActivitiesXPath => new[]
        {
            _aliasProvider.GetHomePage(),
            _aliasProvider.GetGroupOverviewPage(),
            _aliasProvider.GetGroupRoomPage()
        };

        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public GroupFeedLinksProvider(
            IActivityPageHelperFactory pageHelperFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            IDocumentTypeAliasProvider aliasProvider) : base(pageHelperFactory)
        {
            _intranetUserContentHelper = intranetUserContentHelper;
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
                Creator = _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(activity.CreatorId),
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
                Creator = _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(model.CreatorId),
                DetailsNoId = helper.GetDetailsPageUrl().AddGroupId(model.GroupId)
            };
        }
    }
}
