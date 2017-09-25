using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public class ActivityTypeHelper : IActivityTypeHelper
    {
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public ActivityTypeHelper(IIntranetActivityRepository activityRepository,
            IActivityTypeProvider activityTypeProvider)
        {
            _activityRepository = activityRepository;
            _activityTypeProvider = activityTypeProvider;
        }

        public IIntranetType GetActivityType(Guid activityId)
        {
            int typeId = _activityRepository.Get(activityId).Type;
            return _activityTypeProvider.Get(typeId);
        }
    }
}