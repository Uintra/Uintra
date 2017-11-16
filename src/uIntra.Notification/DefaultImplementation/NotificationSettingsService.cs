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

        public void Save<T>(NotifierSettingModel<T> setting)
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
                EmailNotifierSetting = NotifierSetting<EmailNotifierTemplate>(
                    settingsDictionary[NotifierTypeEnum.EmailNotifier],
                    activityType,
                    notificationType,
                    NotifierTypeEnum.EmailNotifier),
                UiNotifierSetting = NotifierSetting<UiNotifierTemplate>(
                    settingsDictionary[NotifierTypeEnum.UiNotifier],
                    activityType,
                    notificationType,
                    NotifierTypeEnum.UiNotifier)
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

        private NotifierSettingModel<T> NotifierSetting<T>(
            NotificationSetting notificationSetting,
            IntranetActivityTypeEnum activityType,
            NotificationTypeEnum notificationType,
            NotifierTypeEnum notifierType) =>
            new NotifierSettingModel<T>
            {
                ActivityType = activityType,
                NotificationType = notificationType,
                NotifierType = notifierType,
                IsEnabled = notificationSetting.IsEnabled,
                Template = notificationSetting.JsonData.Deserialize<T>()
            };

        private NotificationSetting GetUpdatedSetting<T>(NotificationSetting setting, NotifierSettingModel<T> notifierSettingModel)
        {
            setting.JsonData = notifierSettingModel.Template.ToJson();
            setting.IsEnabled = notifierSettingModel.IsEnabled;
            return setting;
        }

    }
}