using System;

namespace Uintra.Core.Activity
{
    public interface ICacheableIntranetActivityService<out TActivity> : ITypedService where TActivity : IIntranetActivity
    {
        TActivity UpdateActivityCache(Guid activityId);
    }
}