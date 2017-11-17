using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Services;
using uIntra.Notification.Core.Sql;

namespace uIntra.Notification
{
    public class NotificationSettingsService : INotificationSettingsService
    {
        private readonly ISqlRepository<NotificationSetting> _repository;
        private readonly IBackofficeNotificationSettingsProvider<EmailNotifierTemplate> _emailNotifierTemplateProvider;
        private readonly IBackofficeNotificationSettingsProvider<UiNotifierTemplate> _uiNotifierTemplateProvider;

        public NotificationSettingsService(ISqlRepository<NotificationSetting> repository,
            IBackofficeNotificationSettingsProvider<EmailNotifierTemplate> emailNotifierTemplateProvider,
            IBackofficeNotificationSettingsProvider<UiNotifierTemplate> uiNotifierTemplateProvider)
        {
            _repository = repository;
            _emailNotifierTemplateProvider = emailNotifierTemplateProvider;
            _uiNotifierTemplateProvider = uiNotifierTemplateProvider;
        }

        public NotifierSettingsModel Get(ActivityEventIdentity activityEventIdentity)
        {
            // --- we have no unit tests ---
            var testTemplate = _uiNotifierTemplateProvider.GetBackofficeSettings(activityEventIdentity);
            var testTemplate1 = _emailNotifierTemplateProvider.GetBackofficeSettings(activityEventIdentity);
            // ---   ---

            var existSettings = _repository.FindAll(s =>
                    s.ActivityType == activityEventIdentity.ActivityType &&
                    s.NotificationType == activityEventIdentity.NotificationType)
                .ToList();

            var absentSettings = GetAbsentSettings(existSettings, activityEventIdentity);
            var absentSettingsList = absentSettings as IList<NotificationSetting> ?? absentSettings.ToList();
            var notifierSettings = GetMappedSettings(existSettings, absentSettingsList, activityEventIdentity);

            _repository.Add(absentSettingsList);

            return notifierSettings;
        }

        public void Save<T>(NotifierSettingModel<T> setting) where T : INotifierTemplate
        {
            var entry = _repository.Find(s =>
                s.ActivityType == setting.ActivityType &&
                s.NotificationType == setting.NotificationType &&
                setting.NotifierType == s.NotifierType);
            var updatedSetting = GetUpdatedSetting(entry, setting);
            _repository.Update(updatedSetting);
        }

        private IEnumerable<NotificationSetting> GetAbsentSettings(
            IEnumerable<NotificationSetting> existingSettings,
            ActivityEventIdentity activityEventIdentity) =>
            GetAbsentSettingsTypes(existingSettings)
                .Select(type => CreateSetting(type, activityEventIdentity));

        private NotifierSettingsModel GetMappedSettings(
            IEnumerable<NotificationSetting> existingEntities,
            IEnumerable<NotificationSetting> absentSettings,
            ActivityEventIdentity activityEventIdentity)
        {
            var settingsDictionary = existingEntities
                .Concat(absentSettings)
                .ToDictionary(e => e.NotifierType);

            var notifierSettings = new NotifierSettingsModel
            {
                EmailNotifierSetting = NotifierSetting<EmailNotifierTemplate>(settingsDictionary[NotifierTypeEnum.EmailNotifier],
                    activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.EmailNotifier)),
                UiNotifierSetting = NotifierSetting<UiNotifierTemplate>(settingsDictionary[NotifierTypeEnum.UiNotifier],
                    activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.UiNotifier))
            };

            return notifierSettings;
        }



        private IEnumerable<NotifierTypeEnum> GetAbsentSettingsTypes(IEnumerable<NotificationSetting> existingEntities)
        {
            var existingEntitiesList = existingEntities as IList<NotificationSetting> ?? existingEntities.ToList();
            var existingNotifierTypes = existingEntitiesList.Select(e => e.NotifierType);
            var absentNotifierTypes = EnumExtensions.GetEnumCases<NotifierTypeEnum>().Except(existingNotifierTypes);

            return absentNotifierTypes;
        }

        private NotificationSetting CreateSetting(
            NotifierTypeEnum notifierType,
            ActivityEventIdentity activityEventIdentity)
        {
            string jsonData;

            switch (notifierType)
            {
                case NotifierTypeEnum.EmailNotifier:
                    var defaultTemplate = new EmailNotifierTemplate()
                    {
                        Content = "content",
                        Subject = "subject"
                    };
                    jsonData = defaultTemplate.ToJson();
                    break;
                case NotifierTypeEnum.UiNotifier:
                    var uiNotifierTemplate = new UiNotifierTemplate()
                    {
                        Message = "msg"
                    };
                    jsonData = uiNotifierTemplate.ToJson();
                    break;
                default:
                    jsonData = string.Empty;
                    break;
            }


            return new NotificationSetting
            {
                Id = Guid.NewGuid(),
                NotifierType = notifierType,
                ActivityType = activityEventIdentity.ActivityType,
                NotificationType = activityEventIdentity.NotificationType,
                IsEnabled = true,
                JsonData = jsonData
            };
        }

        private NotifierSettingModel<T> NotifierSetting<T>(NotificationSetting notificationSetting, ActivityEventNotifierIdentity identity)
            where T : INotifierTemplate 
            => new NotifierSettingModel<T>
            {
                ActivityType = identity.Event.ActivityType,
                NotificationType = identity.Event.NotificationType,
                NotifierType = identity.NotifierType,
                IsEnabled = notificationSetting.IsEnabled,
                NotificationInfo = "NotificationInfo",
                Template = notificationSetting.JsonData.Deserialize<T>()
            };

        private NotificationSetting GetUpdatedSetting<T>(NotificationSetting setting, NotifierSettingModel<T> notifierSettingModel)
            where T : INotifierTemplate
        {
            setting.JsonData = notifierSettingModel.Template.ToJson();
            setting.IsEnabled = notifierSettingModel.IsEnabled;
            return setting;
        }

    }
}