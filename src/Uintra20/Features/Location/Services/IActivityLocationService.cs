using System;
using System.Threading.Tasks;
using Uintra20.Features.Location.Models;

namespace Uintra20.Features.Location.Services
{
    public interface IActivityLocationService
    {
        ActivityLocation Get(Guid activityId);
        void Set(Guid activityId, ActivityLocation location);
        void DeleteForActivity(Guid activityId);


        Task<ActivityLocation> GetAsync(Guid activityId);
        Task SetAsync(Guid activityId, ActivityLocation location);
        Task DeleteForActivityAsync(Guid activityId);
    }
}
