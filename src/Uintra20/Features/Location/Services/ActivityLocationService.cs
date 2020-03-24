using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Features.Location.Models;
using Uintra20.Features.Location.Sql;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Location.Services
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

            if (oldLocation is null)
            {
                if (location is null)
                    return;

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
                if (location?.Address == null || location.ShortAddress == null)
                {
                    _locationRepository.Delete(oldLocation);
                }
                else
                {
                    oldLocation.Address = location.Address;
                    oldLocation.ShortAddress = location.ShortAddress;
                    _locationRepository.Update(oldLocation);
                }
            }
        }

        public void DeleteForActivity(Guid activityId)
        {
            _locationRepository.Delete(l => l.ActivityId == activityId);
        }

        public async Task<ActivityLocation> GetAsync(Guid activityId)
        {
            return (await _locationRepository
                    .AsQueryable()
                    .SingleOrDefaultAsync(l => l.ActivityId == activityId))
                ?.Map<ActivityLocation>();
        }

        public async Task SetAsync(Guid activityId, ActivityLocation location)
        {
            var oldLocation = await _locationRepository
                .AsQueryable()
                .SingleOrDefaultAsync(l => l.ActivityId == activityId);

            if (oldLocation is null)
            {
                if (location is null)
                    return;

                var newLocation = new ActivityLocationEntity()
                {
                    ActivityId = activityId,
                    Address = location.Address,
                    ShortAddress = location.ShortAddress
                };

                await _locationRepository.AddAsync(newLocation);
            }
            else
            {
                if (location?.Address == null || location.ShortAddress == null)
                {
                    await _locationRepository.DeleteAsync(oldLocation.Id);
                }
                else
                {
                    oldLocation.Address = location.Address;
                    oldLocation.ShortAddress = location.ShortAddress;
                    await _locationRepository.UpdateAsync(oldLocation);
                }
            }
        }

        public async Task DeleteForActivityAsync(Guid activityId)
        {
            await _locationRepository.DeleteAsync(l => l.ActivityId == activityId);
        }
    }
}