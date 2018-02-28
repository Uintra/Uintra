using System;
using Uintra.Core.PagePromotion;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Activity
{
    public class PagePromotionPageHelper : IActivityPageHelper
    {
        public Enum ActivityType { get; }

        private readonly IPagePromotionService<PagePromotionBase> _pagePromotionService;

        public PagePromotionPageHelper(Enum activityType, IPagePromotionService<PagePromotionBase> pagePromotionService)
        {
            ActivityType = activityType;

            _pagePromotionService = pagePromotionService;
        }

        public string GetDetailsPageUrl(Guid? activityId = null) =>
            activityId.HasValue ? _pagePromotionService.Get(activityId.Value).Url : null;

        public string GetFeedUrl() => null;

        public string GetOverviewPageUrl() => null;

        public string GetCreatePageUrl() => null;

        public string GetEditPageUrl(Guid activityId) => null;
    }
}