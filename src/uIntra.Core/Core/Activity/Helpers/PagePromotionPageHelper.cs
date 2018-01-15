using System;
using uIntra.Core.PagePromotion;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public class PagePromotionPageHelper : IActivityPageHelper
    {
        public IIntranetType ActivityType { get; }

        private readonly IPagePromotionService<PagePromotionBase> _pagePromotionService;

        public PagePromotionPageHelper(IIntranetType activityType, IPagePromotionService<PagePromotionBase> pagePromotionService)
        {
            ActivityType = activityType;

            _pagePromotionService = pagePromotionService;
        }

        public string GetDetailsPageUrl(Guid? activityId = null) =>
            activityId.HasValue ? _pagePromotionService.Get(activityId.Value).Url : null;

        public string GetOverviewPageUrl() => null;

        public string GetCreatePageUrl() => null;

        public string GetEditPageUrl(Guid activityId) => null;
    }
}