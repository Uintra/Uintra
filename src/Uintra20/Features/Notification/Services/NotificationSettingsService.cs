using System;
using System.Threading.Tasks;
using UBaseline.Core.Extensions;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.Configuration;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Features.Notification.Sql;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Notification.Services
{
    public class NotificationSettingsService : INotificationSettingsService
    {
        private readonly ISqlRepository<NotificationSetting> _repository;
        private readonly IBackofficeNotificationSettingsProvider _backofficeNotificationSettingsProvider;

        public NotificationSettingsService(
            ISqlRepository<NotificationSetting> repository,
            IBackofficeNotificationSettingsProvider backofficeNotificationSettingsProvider)
        {
            _repository = repository;
            _backofficeNotificationSettingsProvider = backofficeNotificationSettingsProvider;
        }

        #region async

        public async Task<NotifierSettingsModel> GetSettingsAsync(ActivityEventIdentity activityEventIdentity)
        {
            var emailNotifierSetting = await GetAsync<EmailNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.EmailNotifier));
            var uiNotifierSetting = await GetAsync<UiNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.UiNotifier));
            var popupNotifierSetting = await GetAsync<PopupNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.PopupNotifier));
            var desktopNotifierSetting = await GetAsync<DesktopNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.DesktopNotifier));
            
            var notifierSettings = new NotifierSettingsModel
            {
                EmailNotifierSetting = emailNotifierSetting,
                UiNotifierSetting = uiNotifierSetting,
                PopupNotifierSetting = popupNotifierSetting,
                DesktopNotifierSetting = desktopNotifierSetting
            };

            return notifierSettings;
        }

        public async Task<NotifierSettingModel<T>> GetAsync<T>(ActivityEventNotifierIdentity activityEventNotifierIdentity) where T : INotifierTemplate
        {
            var defaultTemplate = await _backofficeNotificationSettingsProvider.GetAsync<T>(activityEventNotifierIdentity);

            if (defaultTemplate == null)
            {
                return null;
            }

            var (setting, _) = await FindOrCreateSettingAsync<T>(activityEventNotifierIdentity);

            var mappedSetting = MappedNotifierSetting(setting, activityEventNotifierIdentity, defaultTemplate);

            return mappedSetting;
        }

        public async Task SaveAsync<T>(NotifierSettingModel<T> settingModel) where T : INotifierTemplate
        {
            var identity = new ActivityEventIdentity(settingModel.ActivityType, settingModel.NotificationType)
                .AddNotifierIdentity(settingModel.NotifierType);

            var (setting, isCreated) = await FindOrCreateSettingAsync<T>(identity);

            var updatedSetting = GetUpdatedSetting(setting, settingModel);

            if (isCreated)
            {
                await _repository.AddAsync(updatedSetting);
            }
            else
            {
                await _repository.UpdateAsync(updatedSetting);
            }
        }

        protected virtual async Task<(NotificationSetting setting, bool isCreated)> FindOrCreateSettingAsync<T>(ActivityEventNotifierIdentity identity)
            where T : INotifierTemplate
        {
            var defaults = await _backofficeNotificationSettingsProvider.GetAsync<T>(identity);
            var entry = await FindAsync(identity);

            var setting = entry ?? NewSetting(identity, defaults);
            return (setting, entry is null);
        }

        protected virtual async Task<NotificationSetting> FindAsync(ActivityEventNotifierIdentity activityEventNotifierIdentity)
        {
            var notifierId = activityEventNotifierIdentity.NotifierType.ToInt();
            var notificationId = activityEventNotifierIdentity.Event.NotificationType.ToInt();
            var activityTypeId = activityEventNotifierIdentity.Event.ActivityType.ToInt();

            return await _repository.FindAsync(s =>
                s.ActivityType == activityTypeId &&
                s.NotificationType == notificationId &&
                s.NotifierType == notifierId);
        }
        
        #endregion

        #region sync

        public virtual NotifierSettingsModel GetSettings(ActivityEventIdentity activityEventIdentity)
        {
            var emailNotifierSetting = Get<EmailNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.EmailNotifier));
            var uiNotifierSetting = Get<UiNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.UiNotifier));
            var popupNotifierSetting = Get<PopupNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.PopupNotifier));
            var desktopNotifierSetting = Get<DesktopNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.DesktopNotifier));
            var notifierSettings = new NotifierSettingsModel
            {
                EmailNotifierSetting = emailNotifierSetting,
                UiNotifierSetting = uiNotifierSetting,
                PopupNotifierSetting = popupNotifierSetting,
                DesktopNotifierSetting = desktopNotifierSetting
            };

            return notifierSettings;
        }

        public virtual NotifierSettingModel<T> Get<T>(ActivityEventNotifierIdentity activityEventNotifierIdentity) where T : INotifierTemplate
        {
            var defaultTemplate = _backofficeNotificationSettingsProvider.Get<T>(activityEventNotifierIdentity);

            if (defaultTemplate == null)
            {
                return null;
            }

            var (setting, _) = FindOrCreateSetting<T>(activityEventNotifierIdentity);

            var mappedSetting = MappedNotifierSetting(setting, activityEventNotifierIdentity, defaultTemplate);

            return mappedSetting;
        }

        public virtual void Save<T>(NotifierSettingModel<T> settingModel) where T : INotifierTemplate
        {
            var identity = new ActivityEventIdentity(settingModel.ActivityType, settingModel.NotificationType)
                .AddNotifierIdentity(settingModel.NotifierType);

            var (setting, isCreated) = FindOrCreateSetting<T>(identity);

            var updatedSetting = GetUpdatedSetting(setting, settingModel);

            if (isCreated)
            {
                _repository.Add(updatedSetting);
            }
            else
            {
                _repository.Update(updatedSetting);
            }
        }

        protected virtual NotificationSetting Find(ActivityEventNotifierIdentity activityEventNotifierIdentity)
        {
            var notifierId = activityEventNotifierIdentity.NotifierType.ToInt();
            var notificationId = activityEventNotifierIdentity.Event.NotificationType.ToInt();
            var activityTypeId = activityEventNotifierIdentity.Event.ActivityType.ToInt();

            return _repository.Find(s =>
                            s.ActivityType == activityTypeId &&
                            s.NotificationType == notificationId &&
                            s.NotifierType == notifierId);
        }


        protected virtual (NotificationSetting setting, bool isCreated) FindOrCreateSetting<T>(ActivityEventNotifierIdentity identity)
            where T : INotifierTemplate
        {
            var defaults = _backofficeNotificationSettingsProvider.Get<T>(identity);
            var entry = Find(identity);
            var setting = entry ?? NewSetting(identity, defaults);
            return (setting, entry is null);
        }

        protected virtual NotificationSetting NewSetting<T>(ActivityEventNotifierIdentity identity,
            NotificationSettingDefaults<T> defaults) where T : INotifierTemplate
        {
            return new NotificationSetting
            {
                Id = Guid.NewGuid(),
                NotifierType = identity.NotifierType.ToInt(),
                ActivityType = identity.Event.ActivityType.ToInt(),
                NotificationType = identity.Event.NotificationType.ToInt(),
                IsEnabled = true,
                JsonData = defaults.Template.ToJson()
            };
        }

        protected virtual NotifierSettingModel<T> MappedNotifierSetting<T>(
            NotificationSetting notificationSetting,
            ActivityEventNotifierIdentity identity,
            NotificationSettingDefaults<T> defaults)
            where T : INotifierTemplate
        {

            return new NotifierSettingModel<T>
            {
                ActivityType = identity.Event.ActivityType,
                ActivityTypeName = identity.Event.ActivityType.ToString(),
                NotificationType = identity.Event.NotificationType,
                NotificationTypeName = identity.Event.NotificationType.ToString(),
                NotifierType = identity.NotifierType,
                IsEnabled = notificationSetting.IsEnabled,
                NotificationInfo = defaults.Label,
                Template = notificationSetting.JsonData.Deserialize<T>()
            };
        }

        protected virtual NotificationSetting GetUpdatedSetting<T>(NotificationSetting setting, NotifierSettingModel<T> notifierSettingModel)
            where T : INotifierTemplate
        {
            setting.JsonData = notifierSettingModel.Template.ToJson();
            setting.IsEnabled = notifierSettingModel.IsEnabled;
            return setting;
        }

        #endregion

    }
}