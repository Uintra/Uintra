using System.Web.Http;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;
using Umbraco.Web.WebApi;

namespace uIntra.Notification.Web
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
            var activityEventIdentity = new ActivityEventIdentity(_activityTypeProvider[activityType], _notificationTypeProvider[notificationType]);
            return _notificationSettingsService.GetSettings(activityEventIdentity);
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
            notifierSettingModel.ActivityType = _activityTypeProvider[notifierSettingSaveModel.ActivityType];
            notifierSettingModel.NotificationType = _notificationTypeProvider[notifierSettingSaveModel.NotificationType];
            notifierSettingModel.NotifierType = _notifierTypeProvider[notifierSettingSaveModel.NotifierType];
        }
    }
}