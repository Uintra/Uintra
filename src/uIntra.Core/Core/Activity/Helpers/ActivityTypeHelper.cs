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
            //TODO what about using next code
            // var activityServices = DependencyResolver.Current.GetServices<IIntranetActivityService<IIntranetActivity>>();
            // var activityType = activityServices.Select(service => service.Get(activityId.Value)).Single(a => a != null).Type;
            // TODO: we can even omit first line 
            // and inject IEnumerable<IIntranetActivityService<IIntranetActivity>> to the constructor

            var typeId = GetActivityTypeId(activityId);
            return _activityTypeProvider.Get(typeId);
        }

        private int GetActivityTypeId(Guid activityId)
        {
            var activityTypeId = _activityRepository.Get(activityId)?.Type;
            return activityTypeId ?? _pagePromotionService.Get(activityId).Type.Id;
        }
    }
}