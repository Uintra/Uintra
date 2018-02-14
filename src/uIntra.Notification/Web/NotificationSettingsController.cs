using System.Web.Http;
using uIntra.Notification;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;
using Umbraco.Web.WebApi;

namespace Uintra.Notification.Web
{
    public abstract class NotificationSettingsApiControllerBase : UmbracoAuthorizedApiController
    {
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly INotifierTypeProvider _notifierTypeProvider;


        protected NotificationSettingsApiControllerBase(
            INotificationSettingsService notificationSettingsService,
            IActivityTypeProvider activityTypeProvider,
            INotificationTypeProvider notificationTypeProvider,
            INotifierTypeProvider notifierTypeProvider)
        {
            _notificationSettingsService = notificationSettingsService;
            _activityTypeProvider = activityTypeProvider;
            _notificationTypeProvider = notificationTypeProvider;
            _notifierTypeProvider = notifierTypeProvider;
        }

        [HttpGet]
        public virtual NotifierSettingsModel Get(int activityType, int notificationType)
        {
            var isCommunicationSettings = activityType == CommunicationTypeEnum.CommunicationSettings.ToInt();
            var actType = isCommunicationSettings//TODO: temporary for communication settings
                ? CommunicationTypeEnum.CommunicationSettings
                : _activityTypeProvider[activityType];

            var activityEventIdentity = new ActivityEventIdentity(actType, _notificationTypeProvider[notificationType]);

            var  settings =_notificationSettingsService.GetSettings(activityEventIdentity);

            if (isCommunicationSettings)
            {
                settings.UiNotifierSetting = null;
            }

            return settings;
        }

        [HttpPost]
        public virtual void SaveUiNotifierSetting(NotifierSettingSaveModel<UiNotifierTemplate> notifierSettingModel)
        {
            var mappedModel = notifierSettingModel.Map<NotifierSettingModel<UiNotifierTemplate>>();
            FillEnumTypes(mappedModel, notifierSettingModel);

            _notificationSettingsService.Save(mappedModel);
        }

        [HttpPost]
        public virtual void SaveEmailNotifierSetting(NotifierSettingSaveModel<EmailNotifierTemplate> notifierSettingModel)
        {
            var mappedModel = notifierSettingModel.Map<NotifierSettingModel<EmailNotifierTemplate>>();
            FillEnumTypes(mappedModel, notifierSettingModel);

            _notificationSettingsService.Save(mappedModel);
        }

        protected virtual void FillEnumTypes<T>(
            NotifierSettingModel<T> notifierSettingModel,
            NotifierSettingSaveModel<T> notifierSettingSaveModel)
            where T : INotifierTemplate
        {
            notifierSettingModel.ActivityType = notifierSettingSaveModel.ActivityType == CommunicationTypeEnum.CommunicationSettings.ToInt()
                ? CommunicationTypeEnum.CommunicationSettings
                : _activityTypeProvider[notifierSettingSaveModel.ActivityType];//TODO: temporary for communication settings

            notifierSettingModel.NotificationType = _notificationTypeProvider[notifierSettingSaveModel.NotificationType];
            notifierSettingModel.NotifierType = _notifierTypeProvider[notifierSettingSaveModel.NotifierType];
        }
    }
}