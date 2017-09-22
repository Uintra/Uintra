using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using umbraco;

namespace uIntra.CentralFeed
{
    public class CentralFeedLinksProvider : FeedLinkService, ICentralFeedLinksProvider
    {
        protected override IEnumerable<string> FeedActivitiesXPath => new[]
        {
            _aliasProvider.GetHomePage()
        };

        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public CentralFeedLinksProvider(
            IActivityPageHelperFactory pageHelperFactory,
            IIntranetUserContentHelper intranetUserContentHelper,
            IDocumentTypeAliasProvider aliasProvider)
            : base(pageHelperFactory)
        {
            _intranetUserContentHelper = intranetUserContentHelper;
            _aliasProvider = aliasProvider;
        }

        public IActivityLinks GetLinks(ActivityTransferModel activity)
        {
            IActivityPageHelper helper = GetPageHelper(activity.Type);

            return new ActivityLinks()
            {
                Overview =  helper.GetOverviewPageUrl(),
                Create = helper.GetCreatePageUrl(),
                Details = helper.GetDetailsPageUrl().AddIdParameter(activity.Id),
                Edit = helper.GetEditPageUrl().AddIdParameter(activity.Id),
                Creator = _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(activity.CreatorId),
                DetailsNoId = helper.GetDetailsPageUrl()
            }

                ;
        }

        public IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model)
        {
            IActivityPageHelper helper = GetPageHelper(model.Type);

            return new ActivityCreateLinks()
            {
                Overview = helper.GetOverviewPageUrl(),
                Create = helper.GetCreatePageUrl(),
                Creator = _intranetUserContentHelper.GetProfilePage().Url.AddIdParameter(model.CreatorId),
                DetailsNoId = helper.GetDetailsPageUrl()
            };
        }
    }
}