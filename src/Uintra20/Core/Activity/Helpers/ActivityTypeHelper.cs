using System;
using Uintra20.Core.TypeProviders;

namespace Uintra20.Core.Activity.Helpers
{
    public class ActivityTypeHelper : IActivityTypeHelper
    {
        private readonly IIntranetActivityRepository _activityRepository;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public ActivityTypeHelper(
            IIntranetActivityRepository activityRepository,
            IActivityTypeProvider activityTypeProvider)
        {
            _activityRepository = activityRepository;
            _activityTypeProvider = activityTypeProvider;
        }

        public Enum GetActivityType(Guid activityId)
        {
            var typeId = _activityRepository.Get(activityId)?.Type;
            if (typeId.HasValue) return _activityTypeProvider[typeId.Value];
            return IntranetActivityTypeEnum.ContentPage;
        }
    }
}