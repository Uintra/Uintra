using System;
using Uintra.Core.Activity.Entities;

namespace Uintra.Core.Activity
{
    public interface ICacheableIntranetActivityService<out TActivity> : ITypedService where TActivity : IIntranetActivity
    {
        TActivity UpdateActivityCache(Guid activityId);
        //Task<TActivity> UpdateActivityCacheAsync(Guid activityId);
    }
}
