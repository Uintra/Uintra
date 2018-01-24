using System;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Sql;

namespace uIntra.Notification
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

        public virtual NotifierSettingsModel GetSettings(ActivityEventIdentity activityEventIdentity)
        {
            var emailNotifierSetting = Get<EmailNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.EmailNotifier));
            var uiNotifierSetting = Get<UiNotifierTemplate>(activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.UiNotifier));
            var notifierSettings = new NotifierSettingsModel
            {
                EmailNotifierSetting = emailNotifierSetting,
                UiNotifierSetting = uiNotifierSetting
            };

            return notifierSettings;
        }

        public virtual NotifierSettingModel<T> Get<T>(ActivityEventNotifierIdentity activityEventNotifierIdentity) where T : INotifierTemplate
        {
            var defaultTemplate = _backofficeNotificationSettingsProvider.Get<T>(activityEventNotifierIdentity);
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

            return _repository.Find(s =>
                            s.ActivityType == activityEventNotifierIdentity.Event.ActivityType.Id &&
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
                ActivityType = identity.Event.ActivityType.Id,
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
                NotificationType = identity.Event.NotificationType,
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
    }
}