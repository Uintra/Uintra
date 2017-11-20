using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            var defaultUiNotifierTemplate = _uiNotifierTemplateProvider.GetSettings(activityEventIdentity);
            var defaultEmailNotifierTemplate = _emailNotifierTemplateProvider.GetSettings(activityEventIdentity);
            // ---   ---

            var settingsDefaults = new Dictionary<NotifierTypeEnum, NotificationSettingDefaults<INotifierTemplate>>
            {
                {
                    NotifierTypeEnum.EmailNotifier,
                    new NotificationSettingDefaults<INotifierTemplate>(defaultEmailNotifierTemplate.Label, defaultEmailNotifierTemplate.Template)
                },
                {
                    NotifierTypeEnum.UiNotifier,
                    new NotificationSettingDefaults<INotifierTemplate>(defaultUiNotifierTemplate.Label, defaultUiNotifierTemplate.Template)
                }
            };

            var existingSettings = _repository.FindAll(s =>
                    s.ActivityType == activityEventIdentity.ActivityType &&
                    s.NotificationType == activityEventIdentity.NotificationType)
                .ToList();

            var absentSettingsTypes = GetAbsentSettingsTypes(existingSettings);
            var absentSettings = absentSettingsTypes
                .Select(type => CreateSetting(activityEventIdentity.AddNotifierIdentity(type), settingsDefaults));

            var absentSettingsList = absentSettings as IList<NotificationSetting> ?? absentSettings.ToList();
            var notifierSettings = GetMappedSettings(existingSettings, absentSettingsList, activityEventIdentity, settingsDefaults);

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

        private NotifierSettingsModel GetMappedSettings(
            IEnumerable<NotificationSetting> existingEntities,
            IEnumerable<NotificationSetting> absentSettings,
            ActivityEventIdentity activityEventIdentity,
            IDictionary<NotifierTypeEnum, NotificationSettingDefaults<INotifierTemplate>> settingsDefaults)
        {
            var settingsDictionary = existingEntities
                .Concat(absentSettings)
                .ToDictionary(e => e.NotifierType);

            var notifierSettings = new NotifierSettingsModel
            {
                EmailNotifierSetting = NotifierSetting<EmailNotifierTemplate>(settingsDictionary[NotifierTypeEnum.EmailNotifier],
                    activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.EmailNotifier), settingsDefaults),
                UiNotifierSetting = NotifierSetting<UiNotifierTemplate>(settingsDictionary[NotifierTypeEnum.UiNotifier],
                    activityEventIdentity.AddNotifierIdentity(NotifierTypeEnum.UiNotifier), settingsDefaults)
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

        private NotificationSetting CreateSetting(ActivityEventNotifierIdentity identity,
            Dictionary<NotifierTypeEnum, NotificationSettingDefaults<INotifierTemplate>> dict)
        {
            return new NotificationSetting
            {
                Id = Guid.NewGuid(),
                NotifierType = identity.NotifierType,
                ActivityType = identity.Event.ActivityType,
                NotificationType = identity.Event.NotificationType,
                IsEnabled = true,
                JsonData = dict[identity.NotifierType].Template.ToJson()
            };
        }

        private NotifierSettingModel<T> NotifierSetting<T>(
            NotificationSetting notificationSetting,
            ActivityEventNotifierIdentity identity,
            IDictionary<NotifierTypeEnum, NotificationSettingDefaults<INotifierTemplate>> settingsDefaults)
            where T : INotifierTemplate
        {

            return new NotifierSettingModel<T>
            {
                ActivityType = identity.Event.ActivityType,
                NotificationType = identity.Event.NotificationType,
                NotifierType = identity.NotifierType,
                IsEnabled = notificationSetting.IsEnabled,
                NotificationInfo = settingsDefaults[identity.NotifierType].Label,
                Template = notificationSetting.JsonData.Deserialize<T>()
            };
        }

        private NotificationSetting GetUpdatedSetting<T>(NotificationSetting setting, NotifierSettingModel<T> notifierSettingModel)
            where T : INotifierTemplate
        {
            setting.JsonData = notifierSettingModel.Template.ToJson();
            setting.IsEnabled = notifierSettingModel.IsEnabled;
            return setting;
        }

    }
}