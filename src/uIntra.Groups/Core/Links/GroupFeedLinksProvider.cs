using System;
using System.Collections.Generic;
using uIntra.CentralFeed;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
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
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public GroupFeedLinksProvider(
            IActivityPageHelperFactory pageHelperFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IDocumentTypeAliasProvider aliasProvider) : base(pageHelperFactory)
        {
            _intranetUserContentHelper = intranetUserContentHelper;
            _intranetUserService = intranetUserService;
            _aliasProvider = aliasProvider;
        }

        public ActivityLinks GetLinks(IFeedItem item, Guid groupId)
        {
            var helper = GetPageHelper(item.Type);

            return new ActivityLinks(
                overview: helper.GetOverviewPageUrl().AddGroupId(groupId),
                create: helper.GetCreatePageUrl().AddGroupId(groupId),
                details: helper.GetDetailsPageUrl().AddIdParameter(item.Id).AddGroupId(groupId),
                edit: helper.GetEditPageUrl().AddIdParameter(item.Id).AddGroupId(groupId),
                creator: _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(item.CreatorId),
                detailsNoId: helper.GetDetailsPageUrl().AddGroupId(groupId)
            );
        }

        public ActivityCreateLinks GetCreateLinks(IIntranetType type, Guid groupId)
        {
            IActivityPageHelper helper = GetPageHelper(type);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;

            return new ActivityCreateLinks(
                overview: helper.GetOverviewPageUrl().AddGroupId(groupId),
                create: helper.GetCreatePageUrl().AddGroupId(groupId),
                creator: _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(currentUserId),
                detailsNoId: helper.GetDetailsPageUrl().AddGroupId(groupId)
            );
        }
    }
}
