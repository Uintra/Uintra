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
            return typeId.HasValue ? _activityTypeProvider[typeId.Value] : _pagePromotionService.Get(activityId).Type;
        }
    }
}