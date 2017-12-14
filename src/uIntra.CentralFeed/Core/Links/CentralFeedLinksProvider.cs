using System.Collections.Generic;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.PagePromotion;

namespace uIntra.CentralFeed
{
    public class CentralFeedLinkProvider : FeedLinkProvider, ICentralFeedLinkProvider
    {
        protected override IEnumerable<string> FeedActivitiesXPath => new[]
        {
            _aliasProvider.GetHomePage()
        };

        private readonly IDocumentTypeAliasProvider _aliasProvider;
        private readonly IPagePromotionService<PagePromotionBase> _pagePromotionService;

        public CentralFeedLinkProvider(
            IActivityPageHelperFactory pageHelperFactory,
            IProfileLinkProvider profileLinkProvider,
            IDocumentTypeAliasProvider aliasProvider,
            IPagePromotionService<PagePromotionBase> pagePromotionService)
            : base(pageHelperFactory, profileLinkProvider)
        {
            _aliasProvider = aliasProvider;
            _pagePromotionService = pagePromotionService;
        }

        public IActivityLinks GetLinks(ActivityTransferModel activity)
        {
            if (activity.Type.Id == IntranetActivityTypeEnum.PagePromotion.ToInt())
            {
                return GetPagePromotionLinks(activity);
            }

            IActivityPageHelper helper = GetPageHelper(activity.Type);

            return new ActivityLinks
            {
                Overview = helper.GetOverviewPageUrl(),
                Create = helper.GetCreatePageUrl(),
                Details = helper.GetDetailsPageUrl().AddIdParameter(activity.Id),
                Edit = helper.GetEditPageUrl().AddIdParameter(activity.Id),
                Owner = GetProfileLink(activity.OwnerId),
                DetailsNoId = helper.GetDetailsPageUrl()
            };
        }

        public IActivityCreateLinks GetCreateLinks(ActivityTransferCreateModel model)
        {
            IActivityPageHelper helper = GetPageHelper(model.Type);

            return new ActivityCreateLinks
            {
                Overview = helper.GetOverviewPageUrl(),
                Create = helper.GetCreatePageUrl(),
                Owner = GetProfileLink(model.OwnerId),
                DetailsNoId = helper.GetDetailsPageUrl()
            };
        }

        public IActivityLinks GetPagePromotionLinks(ActivityTransferModel activity)
        {
            return new ActivityLinks
            {
                Details = _pagePromotionService.GetPagePromotion(activity.Id).Url,
                Owner = GetProfileLink(activity.OwnerId)
            };
        }
    }
}