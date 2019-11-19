using System;
using System.Threading.Tasks;
using Uintra20.Features.Activity.Entities;

namespace Uintra20.Features.Activity
{
    public interface ICacheableIntranetActivityService<TActivity> : ITypedService where TActivity : IIntranetActivity
    {
        TActivity UpdateActivityCache(Guid activityId);
        Task<TActivity> UpdateActivityCacheAsync(Guid activityId);
    }
}
