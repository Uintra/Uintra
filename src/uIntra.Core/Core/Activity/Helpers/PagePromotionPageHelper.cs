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

        public string GetDetailsPageUrl(Guid activityId) => _pagePromotionService.Get(activityId).Url;

        public string GetOverviewPageUrl() => null;

        public string GetDetailsPageUrl()
        {
            throw new NotImplementedException();
        }

        public string GetCreatePageUrl() => null;

        public string GetEditPageUrl() => null;
    }
}