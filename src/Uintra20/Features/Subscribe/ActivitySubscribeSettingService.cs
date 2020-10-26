using System;
using System.Threading.Tasks;
using Uintra20.Features.Subscribe.Models;
using Uintra20.Features.Subscribe.Sql;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Subscribe
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

        public virtual Task<ActivitySubscribeSetting> GetAsync(Guid activityId)
        {
            return _activitySubscribeSettingRepository.FindAsync(s => s.ActivityId == activityId);
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

        public virtual async Task<ActivitySubscribeSetting> CreateAsync(ActivitySubscribeSettingDto setting)
        {
            var createSetting = new ActivitySubscribeSetting
            {
                ActivityId = setting.ActivityId,
                CanSubscribe = setting.CanSubscribe,
                SubscribeNotes = setting.SubscribeNotes
            };

            await _activitySubscribeSettingRepository.AddAsync(createSetting);

            return createSetting;
        }

        public virtual void Save(ActivitySubscribeSettingDto setting)
        {
            var updateSetting = Get(setting.ActivityId);
            if (updateSetting == null)
            {
                Create(setting);
            }
            else
            {
                updateSetting.CanSubscribe = setting.CanSubscribe;
                updateSetting.SubscribeNotes = setting.SubscribeNotes;

                _activitySubscribeSettingRepository.Update(updateSetting);
            }
        }

        public virtual async Task SaveAsync(ActivitySubscribeSettingDto setting)
        {
            var updateSetting = await GetAsync(setting.ActivityId);
            if (updateSetting == null)
            {
                await CreateAsync(setting);
            }
            else
            {
                updateSetting.CanSubscribe = setting.CanSubscribe;
                updateSetting.SubscribeNotes = setting.SubscribeNotes;

                await _activitySubscribeSettingRepository.UpdateAsync(updateSetting);
            }
        }

        public virtual void Delete(Guid activityId)
        {
            _activitySubscribeSettingRepository.Delete(setting => setting.ActivityId == activityId);
        }

        public virtual Task DeleteAsync(Guid activityId)
        {
            return _activitySubscribeSettingRepository.DeleteAsync(setting => setting.ActivityId == activityId);
        }

        public virtual void FillSubscribeSettings(ISubscribeSettings activity)
        {
            var settings = Get(activity.Id);
            if (settings == null) return;

            activity.CanSubscribe = settings.CanSubscribe;
            activity.SubscribeNotes = settings.SubscribeNotes;
        }

        public virtual async Task FillSubscribeSettingsAsync(ISubscribeSettings activity)
        {
            var settings = await GetAsync(activity.Id);
            if (settings == null) return;

            activity.CanSubscribe = settings.CanSubscribe;
            activity.SubscribeNotes = settings.SubscribeNotes;
        }
    }
}