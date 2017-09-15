using System;
using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;

namespace uIntra.CentralFeed
{
    public class CentralFeedLinkService : FeedLinkService, ICentralFeedLinkService
    {
        protected override IEnumerable<string> FeedActivitiesXPath => new[]
        {
            _aliasProvider.GetHomePage()
        };

        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public CentralFeedLinkService(
            IActivityPageHelperFactory pageHelperFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            IDocumentTypeAliasProvider aliasProvider,
            IIntranetUserService<IIntranetUser> intranetUserService)
            : base(pageHelperFactory)
        {
            _intranetUserContentHelper = intranetUserContentHelper;
            _aliasProvider = aliasProvider;
            _intranetUserService = intranetUserService;
        }

        public ActivityLinks GetLinks(IFeedItem item)
        {
            IActivityPageHelper helper = GetPageHelper(item.Type);

            return new ActivityLinks(
                    overview: helper.GetOverviewPageUrl(),
                    create: helper.GetCreatePageUrl(),
                    details: helper.GetDetailsPageUrl().AddIdParameter(item.Id),
                    edit: helper.GetEditPageUrl().AddIdParameter(item.Id),
                    creator: _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(item.CreatorId),
                    detailsNoId: helper.GetDetailsPageUrl()
                );
        }

        public ActivityCreateLinks GetCreateLinks(IIntranetType type)
        {
            IActivityPageHelper helper = GetPageHelper(type);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            return new ActivityCreateLinks(
                    overview: helper.GetOverviewPageUrl(),
                    create: helper.GetCreatePageUrl(),
                    creator: _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(currentUserId),
                    detailsNoId: helper.GetDetailsPageUrl()
                );
        }
    }
}