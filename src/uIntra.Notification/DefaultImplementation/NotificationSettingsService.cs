using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core;
using uIntra.Notification.Core.Services;
using uIntra.Notification.Core.Sql;
using static uIntra.Notification.Configuration.NotifierTypeEnum;

namespace uIntra.Notification
{
    public class NotificationSettingsService : INotificationSettingsService
    {
        private readonly ISqlRepository<NotificationSetting> _repository;
        private readonly IBackofficeNotificationSettingsProvider<EmailNotifierTemplate> _emailNotifierTemplateProvider;
        private readonly IBackofficeNotificationSettingsProvider<UiNotifierTemplate> _uiNotifierTemplateProvider;
        private readonly INotifierTypeProvider _notifierTypeProvider;

        public NotificationSettingsService(ISqlRepository<NotificationSetting> repository,
            IBackofficeNotificationSettingsProvider<EmailNotifierTemplate> emailNotifierTemplateProvider,
            IBackofficeNotificationSettingsProvider<UiNotifierTemplate> uiNotifierTemplateProvider,
            INotifierTypeProvider notifierTypeProvider)
        {
            _repository = repository;
            _emailNotifierTemplateProvider = emailNotifierTemplateProvider;
            _uiNotifierTemplateProvider = uiNotifierTemplateProvider;
            _notifierTypeProvider = notifierTypeProvider;
        }

        public NotifierSettingsModel GetAll(ActivityEventIdentity activityEventIdentity)
        {
            var defaultUiNotifierTemplate = _uiNotifierTemplateProvider.GetSettings(activityEventIdentity);
            var defaultEmailNotifierTemplate = _emailNotifierTemplateProvider.GetSettings(activityEventIdentity);
            var settingsDefaults = new Dictionary<IIntranetType, NotificationSettingDefaults<INotifierTemplate>>(new IntranetTypeComparer())
            {
                {
                    GetIntranetType(EmailNotifier),
                    new NotificationSettingDefaults<INotifierTemplate>(defaultEmailNotifierTemplate.Label, defaultEmailNotifierTemplate.Template)
                },
                {
                    GetIntranetType(UiNotifier),
                    new NotificationSettingDefaults<INotifierTemplate>(defaultUiNotifierTemplate.Label, defaultUiNotifierTemplate.Template)
                }
            };

            var existingSettings = _repository.FindAll(s =>
                    s.ActivityType == activityEventIdentity.ActivityType.Id &&
                    s.NotificationType == activityEventIdentity.NotificationType.Id)
                .ToList();

            var absentSettingsTypes = GetAbsentSettingsTypes(existingSettings);
            var absentSettings = absentSettingsTypes.Select(type => NewSetting(activityEventIdentity.AddNotifierIdentity(type), settingsDefaults[type]));

            var absentSettingsList = absentSettings as IList<NotificationSetting> ?? absentSettings.ToList();
            var notifierSettings = GetMappedSettings(
                existingSettings,
                absentSettingsList,
                activityEventIdentity,
                defaultEmailNotifierTemplate,
                defaultUiNotifierTemplate);

            return notifierSettings;
        }

        public NotifierSettingModel<EmailNotifierTemplate> GetEmailNotifierSettings(ActivityEventNotifierIdentity activityEventNotifierIdentity)
        {
            var defaultEmailNotifierTemplate = _emailNotifierTemplateProvider.GetSettings(activityEventNotifierIdentity.Event);
            var (setting, _) = FindOrCreateSetting(activityEventNotifierIdentity, defaultEmailNotifierTemplate);

            var mappedSetting = MappedNotifierSetting(setting, activityEventNotifierIdentity, defaultEmailNotifierTemplate);

            return mappedSetting;
        }

        public NotifierSettingModel<UiNotifierTemplate> GetUiNotifierSettings(ActivityEventNotifierIdentity activityEventNotifierIdentity)
        {
            var defaultEmailNotifierTemplate = _uiNotifierTemplateProvider.GetSettings(activityEventNotifierIdentity.Event);
            var (setting, _) = FindOrCreateSetting(activityEventNotifierIdentity, defaultEmailNotifierTemplate);

            var mappedSetting = MappedNotifierSetting(setting, activityEventNotifierIdentity, defaultEmailNotifierTemplate);

            return mappedSetting;
        }

        public void SaveEmailNotifierSettings(NotifierSettingModel<EmailNotifierTemplate> settingModel)
        {
            var defaultUiNotifierTemplate = _emailNotifierTemplateProvider
                .GetSettings(new ActivityEventIdentity(settingModel.ActivityType, settingModel.NotificationType));
            Save(settingModel, defaultUiNotifierTemplate);
        }

        public void SaveUiNotifierSettings(NotifierSettingModel<UiNotifierTemplate> settingModel)
        {
            var defaultUiNotifierTemplate = _uiNotifierTemplateProvider
                .GetSettings(new ActivityEventIdentity(settingModel.ActivityType, settingModel.NotificationType));
            Save(settingModel, defaultUiNotifierTemplate);
        }

        private void Save<T>(NotifierSettingModel<T> settingModel, NotificationSettingDefaults<T> defaults) where T : INotifierTemplate
        {
            var identity = new ActivityEventIdentity(settingModel.ActivityType, settingModel.NotificationType)
                .AddNotifierIdentity(settingModel.NotifierType);

            var (setting, isCreated) = FindOrCreateSetting(identity, defaults);

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

        private NotificationSetting Find(ActivityEventNotifierIdentity activityEventNotifierIdentity) =>
            _repository.Find(s =>
                s.ActivityType == activityEventNotifierIdentity.Event.ActivityType.Id &&
                s.NotificationType == activityEventNotifierIdentity.Event.NotificationType.Id &&
                s.NotifierType == activityEventNotifierIdentity.NotifierType.Id);

        private (NotificationSetting setting, bool isCreated) FindOrCreateSetting<T>(
            ActivityEventNotifierIdentity activityEventNotifierIdentity,
            NotificationSettingDefaults<T> defaults)
            where T : INotifierTemplate
        {
            var entry = Find(activityEventNotifierIdentity);
            var setting = entry ?? NewSetting(activityEventNotifierIdentity, defaults);
            return (setting, entry is null);
        }

        private NotifierSettingsModel GetMappedSettings(
            IEnumerable<NotificationSetting> existingEntities,
            IEnumerable<NotificationSetting> absentSettings,
            ActivityEventIdentity activityEventIdentity,
            NotificationSettingDefaults<EmailNotifierTemplate> emailNotifierSettingDefaults,
            NotificationSettingDefaults<UiNotifierTemplate> uiNotifierSettingDefaults)
        {
            var settingsDictionary = existingEntities
                .Concat(absentSettings)
                .ToDictionary(e => _notifierTypeProvider.Get(e.NotifierType), new IntranetTypeComparer());

            var notifierSettings = new NotifierSettingsModel
            {
                EmailNotifierSetting = MappedNotifierSetting(settingsDictionary[GetIntranetType(EmailNotifier)],
                    activityEventIdentity.AddNotifierIdentity(GetIntranetType(EmailNotifier)), emailNotifierSettingDefaults),
                UiNotifierSetting = MappedNotifierSetting(settingsDictionary[GetIntranetType(UiNotifier)],
                    activityEventIdentity.AddNotifierIdentity(GetIntranetType(UiNotifier)), uiNotifierSettingDefaults)
            };
            return notifierSettings;
        }


        private IEnumerable<IIntranetType> GetAbsentSettingsTypes(IEnumerable<NotificationSetting> existingEntities)
        {
            var existingEntitiesList = existingEntities as IList<NotificationSetting> ?? existingEntities.ToList();
            var existingNotifierTypes = existingEntitiesList.Select(e => _notifierTypeProvider.Get(e.NotifierType));
            var absentNotifierTypes = _notifierTypeProvider.GetAll().Except(existingNotifierTypes, new IntranetTypeComparer());

            return absentNotifierTypes;
        }

        private NotificationSetting NewSetting<T>(ActivityEventNotifierIdentity identity,
            NotificationSettingDefaults<T> defaults) where T : INotifierTemplate
        {
            return new NotificationSetting
            {
                Id = Guid.NewGuid(),
                NotifierType = identity.NotifierType.Id,
                ActivityType = identity.Event.ActivityType.Id,
                NotificationType = identity.Event.NotificationType.Id,
                IsEnabled = true,
                JsonData = defaults.Template.ToJson()
            };
        }

        private NotifierSettingModel<T> MappedNotifierSetting<T>(
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

        private NotificationSetting GetUpdatedSetting<T>(NotificationSetting setting, NotifierSettingModel<T> notifierSettingModel)
            where T : INotifierTemplate
        {
            setting.JsonData = notifierSettingModel.Template.ToJson();
            setting.IsEnabled = notifierSettingModel.IsEnabled;
            return setting;
        }

        protected virtual IIntranetType GetIntranetType(NotifierTypeEnum notifierType) => _notifierTypeProvider.Get((int) notifierType);
    }
}