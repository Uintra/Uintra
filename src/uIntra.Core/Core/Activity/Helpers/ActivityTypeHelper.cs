using System;
using BCLExtensions;
using uIntra.Core.Extensions;
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

        //TODO what about using next code
        // var activityServices = DependencyResolver.Current.GetServices<IIntranetActivityService<IIntranetActivity>>();
        // var activityType = activityServices.Select(service => service.Get(activityId.Value)).Single(a => a != null).Type;
        // TODO: we can even omit first line 
        // and inject IEnumerable<IIntranetActivityService<IIntranetActivity>> to the constructor
        public Enum GetActivityType(Guid activityId)
        {
            var typeId = _activityRepository.Get(activityId)?.Type;
            return typeId.HasValue ? _activityTypeProvider[typeId.Value] : _pagePromotionService.Get(activityId).Type;
        }
    }
}