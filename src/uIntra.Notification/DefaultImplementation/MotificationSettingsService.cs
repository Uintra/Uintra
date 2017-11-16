using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Models;
using uIntra.Notification.Core.Services;
using uIntra.Notification.Core.Sql;

namespace uIntra.Notification
{
    public class NotificationSettingsService : INotificationSettingsService
    {
        private readonly ISqlRepository<NotificationSetting> _repository;

        public NotificationSettingsService(ISqlRepository<NotificationSetting> repository)
        {
            _repository = repository;
        }

        public NotifierSettingsModel Get(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            var existSettings = _repository.FindAll(s => s.ActivityType == activityType && s.NotificationType == notificationType).ToList();

            var absentSettings = GetAbsentSettings(existSettings, activityType, notificationType);
            var absentSettingsList = absentSettings as IList<NotificationSetting> ?? absentSettings.ToList();
            var notifierSettings = GetMappedSettings(existSettings, absentSettingsList, activityType, notificationType);

            _repository.Add(absentSettingsList);

            return notifierSettings;
        }

        public void Save(NotifierSettingsModel settings)
        {
            var settingsEntities = _repository.FindAll(s => s.ActivityType == settings.ActivityType && s.NotificationType == settings.NotificationType);

            var updatedSettings = UpdateEntities(settingsEntities, settings);

            _repository.Update(updatedSettings);
        }

        private IEnumerable<NotificationSetting> UpdateEntities(IEnumerable<NotificationSetting> settingsEntities, NotifierSettingsModel settings)
        {
            var settingsEntitiesDictionary = settingsEntities.ToDictionary(e => e.NotifierType);

            var updatedEmailSetting = GetUpdatedSetting(
                settingsEntitiesDictionary[NotifierTypeEnum.EmailNotifier],
                settings.EmailNotifierSetting.IsEnabled,
                settings.EmailNotifierSetting.Template.ToJson());

            var updatedUiSetting = GetUpdatedSetting(
                settingsEntitiesDictionary[NotifierTypeEnum.UiNotifier],
                settings.UiNotifierSetting.IsEnabled,
                settings.UiNotifierSetting.ToJson());

            return new[]
            {
                updatedEmailSetting,
                updatedUiSetting
            };
        }

        private IEnumerable<NotificationSetting> GetAbsentSettings(
            IEnumerable<NotificationSetting> existingSettings,
            IntranetActivityTypeEnum activityType,
            NotificationTypeEnum notificationType) =>
            GetAbsentSettingsTypes(existingSettings)
                .Select(type => CreateSetting(type, activityType, notificationType));

        private NotifierSettingsModel GetMappedSettings(
            IEnumerable<NotificationSetting> existingEntities,
            IEnumerable<NotificationSetting> absentSettings,
            IntranetActivityTypeEnum activityType,
            NotificationTypeEnum notificationType)
        {

            var settingsDictionary = existingEntities
                .Concat(absentSettings)
                .ToDictionary(e => e.NotifierType);

            var notifierSettings = new NotifierSettingsModel
            {
                ActivityType = activityType,
                NotificationType = notificationType,
                EmailNotifierSetting = GetEmailNotifierSetting(settingsDictionary[NotifierTypeEnum.EmailNotifier]),
                UiNotifierSetting = GetUiNotifierSetting(settingsDictionary[NotifierTypeEnum.UiNotifier])
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
            IntranetActivityTypeEnum activityType,
            NotificationTypeEnum notificationType)
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
                ActivityType = activityType,
                NotificationType = notificationType,
                IsEnabled = true,
                JsonData = jsonData
            };
        }

        private EmailNotifierSettingModel GetEmailNotifierSetting(NotificationSetting notificationSetting) =>
            new EmailNotifierSettingModel
            {
                IsEnabled = notificationSetting.IsEnabled,
                Template = notificationSetting.JsonData.Deserialize<EmailNotifierTemplate>()

            };

        private UiNotifierSettingModel GetUiNotifierSetting(NotificationSetting notificationSetting) =>
            new UiNotifierSettingModel
            {
                IsEnabled = notificationSetting.IsEnabled,
                Template = notificationSetting.JsonData.Deserialize<UiNotifierTemplate>()
            };

        private NotificationSetting GetUpdatedSetting(NotificationSetting setting, bool isEnabled, string jsonData)
        {
            setting.JsonData = jsonData;
            setting.IsEnabled = isEnabled;
            return setting;
        }

    }
}