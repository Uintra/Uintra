using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extentions;
using uIntra.Core.Links;

namespace uIntra.CentralFeed
{
    public class CentralFeedLinksProvider : FeedLinkProvider, ICentralFeedLinksProvider
    {
        protected override IEnumerable<string> FeedActivitiesXPath => new[]
        {
            _aliasProvider.GetHomePage()
        };

        private readonly IDocumentTypeAliasProvider _aliasProvider;

        public CentralFeedLinksProvider(
            IActivityPageHelperFactory pageHelperFactory,
            IProfileLinkProvider profileLinkProvider,
            IDocumentTypeAliasProvider aliasProvider)
            : base(pageHelperFactory, profileLinkProvider)
        {
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
                Creator = GetProfileLink(activity.CreatorId),
                DetailsNoId = helper.GetDetailsPageUrl()
            };
        }

        public IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model)
        {
            IActivityPageHelper helper = GetPageHelper(model.Type);

            return new ActivityCreateLinks()
            {
                Overview = helper.GetOverviewPageUrl(),
                Create = helper.GetCreatePageUrl(),
                Creator = GetProfileLink(model.CreatorId),
                DetailsNoId = helper.GetDetailsPageUrl()
            };
        }
    }
}