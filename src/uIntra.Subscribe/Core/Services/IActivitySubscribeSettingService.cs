using System;

namespace uIntra.Subscribe
{
    public interface IActivitySubscribeSettingService
    {
        ActivitySubscribeSetting Get(Guid activityId);
        ActivitySubscribeSetting Create(ActivitySubscribeSettingDto setting);
        void Save(ActivitySubscribeSettingDto setting);
        void FillSubscribeSettings(ISubscribeSettings activity);
    }
}
