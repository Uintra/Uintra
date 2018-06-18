using System;

namespace Uintra.Subscribe
{
    public interface IActivitySubscribeSettingService
    {
        ActivitySubscribeSetting Get(Guid activityId);
        ActivitySubscribeSetting Create(ActivitySubscribeSettingDto setting);
        void Save(ActivitySubscribeSettingDto setting);
        void Delete(Guid activityId);
        void FillSubscribeSettings(ISubscribeSettings activity);
    }
}
