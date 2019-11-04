using System;
using System.Threading.Tasks;

namespace Uintra20.Core.Activity
{
    public interface ICacheableIntranetActivityService<TActivity> : ITypedService where TActivity : IIntranetActivity
    {
        TActivity UpdateActivityCache(Guid activityId);
        Task<TActivity> UpdateActivityCacheAsync(Guid activityId);
    }
}
