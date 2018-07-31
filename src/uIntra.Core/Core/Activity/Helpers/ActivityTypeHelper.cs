using System;
using Uintra.Core.PagePromotion;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Activity
{
    public class ActivityTypeHelper : IActivityTypeHelper
    {
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IPagePromotionService<PagePromotionBase> _pagePromotionService;

        public ActivityTypeHelper(
            IIntranetActivityRepository activityRepository,
            IActivityTypeProvider activityTypeProvider,
            IPagePromotionService<PagePromotionBase> pagePromotionService)
        {
            _activityRepository = activityRepository;
            _activityTypeProvider = activityTypeProvider;
            _pagePromotionService = pagePromotionService;
        }
        
        public Enum GetActivityType(Guid activityId)
        {
            var typeId = _activityRepository.Get(activityId)?.Type;
            if (typeId.HasValue) return _activityTypeProvider[typeId.Value];
            var pagePromotionActivity = _pagePromotionService.Get(activityId);
            if (pagePromotionActivity != null) return pagePromotionActivity.Type;
            return IntranetActivityTypeEnum.ContentPage;
        }
    }
}