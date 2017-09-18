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
            _aliasProvider.GetHomePage()
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

        public ActivityLinks GetLinks(GroupActivityTransferModel activity)
        {
            var helper = GetPageHelper(activity.Type);

            return new ActivityLinks(
                overview: helper.GetOverviewPageUrl().AddGroupId(activity.GroupId),
                create: helper.GetCreatePageUrl()?.AddGroupId(activity.GroupId),
                details: helper.GetDetailsPageUrl().AddIdParameter(activity.Id).AddGroupId(activity.GroupId),
                edit: helper.GetEditPageUrl().AddIdParameter(activity.Id).AddGroupId(activity.GroupId),
                creator: _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(activity.CreatorId),
                detailsNoId: helper.GetDetailsPageUrl().AddGroupId(activity.GroupId)
            );
        }

        public ActivityCreateLinks GetCreateLinks(GroupActivityTransferCreateModel model)
        {
            IActivityPageHelper helper = GetPageHelper(model.Type);

            return new ActivityCreateLinks(
                overview: helper.GetOverviewPageUrl().AddGroupId(model.GroupId),
                create: helper.GetCreatePageUrl()?.AddGroupId(model.GroupId),
                creator: _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(model.CreatorId),
                detailsNoId: helper.GetDetailsPageUrl().AddGroupId(model.GroupId)
            );
        }
    }
}
