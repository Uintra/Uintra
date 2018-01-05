using System;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Location.Entities;
using uIntra.Core.Persistence;

namespace uIntra.Core.Location
{
    public class ActivityLocationService : IActivityLocationService
    {
        private readonly ISqlRepository<int, ActivityLocationEntity> _locationRepository;

        public ActivityLocationService(ISqlRepository<int, ActivityLocationEntity> locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public ActivityLocation Get(Guid activityId)
        {
            return _locationRepository
                .AsQueryable()
                .SingleOrDefault(l => l.ActivityId == activityId)
                ?.Map<ActivityLocation>();
        }

        public void Set(Guid activityId, ActivityLocation location)
        {
            var oldLocation = _locationRepository
                .AsQueryable()
                .SingleOrDefault(l => l.ActivityId == activityId);

            if (oldLocation is default)
            {
                var newLocation = new ActivityLocationEntity()
                {
                    ActivityId = activityId,
                    Address = location.Address,
                    ShortAddress = location.ShortAddress
                };

                _locationRepository.Add(newLocation);
            }
            else
            {
                oldLocation.Address = location.Address;
                oldLocation.ShortAddress = location.ShortAddress;
                _locationRepository.Update(oldLocation);
            }
        }
    }
}