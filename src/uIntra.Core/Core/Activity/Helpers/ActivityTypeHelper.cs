using System;
using uIntra.Core.PagePromotion;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
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

        public IIntranetType GetActivityType(Guid activityId)
        {
            var typeId = GetActivityTypeId(activityId);
            return _activityTypeProvider.Get(typeId);
        }

        private int GetActivityTypeId(Guid activityId)
        {
            var activityTypeId = _activityRepository.Get(activityId)?.Type;
            return activityTypeId ?? _pagePromotionService.GetPagePromotion(activityId).Type.Id;
        }
    }
}