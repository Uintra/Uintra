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
            var notifierSettings = GetMappedSettings(existSettings, absentSettings);

            //_repository.Add(absentSettings);

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
                settings.EmailNotifierSetting.ToJson());

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
            IEnumerable<NotificationSetting> absentSettings)
        {

            var settingsDictionary = existingEntities
                .Concat(absentSettings)
                .ToDictionary(e => e.NotifierType);

            var notifierSettings = new NotifierSettingsModel
            {
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
            NotificationTypeEnum notificationType) =>
            new NotificationSetting
            {
                Id = Guid.NewGuid(),
                NotifierType = notifierType,
                ActivityType = activityType,
                NotificationType = notificationType,
                IsEnabled = true,
                JsonData = String.Empty
            };

        private EmailNotifierSettingModel GetEmailNotifierSetting(NotificationSetting notificationSetting) =>
            new EmailNotifierSettingModel()
            {
                IsEnabled = notificationSetting.IsEnabled
            };

        private UiNotifierSettingModel GetUiNotifierSetting(NotificationSetting notificationSetting) =>
            new UiNotifierSettingModel()
            {
                IsEnabled = notificationSetting.IsEnabled
            };

        private NotificationSetting GetUpdatedSetting(NotificationSetting setting, bool isEnabled, string jsonData)
        {
            setting.JsonData = jsonData;
            setting.IsEnabled = isEnabled;
            return setting;
        }

    }
}