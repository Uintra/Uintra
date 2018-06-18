using System;
using uIntra.Core.Persistence;

namespace uIntra.Subscribe
{
    public class ActivitySubscribeSettingService : IActivitySubscribeSettingService
    {
        private readonly ISqlRepository<int, ActivitySubscribeSetting> _activitySubscribeSettingRepository;

        public ActivitySubscribeSettingService(ISqlRepository<int, ActivitySubscribeSetting> activitySubscribeSettingRepository)
        {
            _activitySubscribeSettingRepository = activitySubscribeSettingRepository;
        }

        public virtual ActivitySubscribeSetting Get(Guid activityId)
        {
            return _activitySubscribeSettingRepository.Find(s => s.ActivityId == activityId);
        }

        public virtual ActivitySubscribeSetting Create(ActivitySubscribeSettingDto setting)
        {
            var createSetting = new ActivitySubscribeSetting
            {
                ActivityId = setting.ActivityId,
                CanSubscribe = setting.CanSubscribe,
                SubscribeNotes = setting.SubscribeNotes
            };

            _activitySubscribeSettingRepository.Add(createSetting);

            return createSetting;
        }

        public virtual void Save(ActivitySubscribeSettingDto setting)
        {
            var updateSetting = Get(setting.ActivityId);
            updateSetting.CanSubscribe = setting.CanSubscribe;
            updateSetting.SubscribeNotes = setting.SubscribeNotes;

            _activitySubscribeSettingRepository.Update(updateSetting);
        }

        public virtual void Delete(Guid activityId)
        {
            _activitySubscribeSettingRepository.Delete(setting => setting.ActivityId == activityId);
        }

        public virtual void FillSubscribeSettings(ISubscribeSettings activity)
        {
            var settings = Get(activity.Id);
            if (settings == null) return;

            activity.CanSubscribe = settings.CanSubscribe;
            activity.SubscribeNotes = settings.SubscribeNotes;
        }
    }
}
