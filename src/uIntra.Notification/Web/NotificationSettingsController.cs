using Umbraco.Web.WebApi;
using System.Web.Http;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Web
{
    public abstract class NotificationSettingsApiControllerBase : UmbracoAuthorizedApiController
    {
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        

        protected NotificationSettingsApiControllerBase(
            INotificationSettingsService notificationSettingsService,
            IActivityTypeProvider activityTypeProvider,
            INotificationTypeProvider notificationTypeProvider)
        {
            _notificationSettingsService = notificationSettingsService;
            _activityTypeProvider = activityTypeProvider;
            _notificationTypeProvider = notificationTypeProvider;
        }

        [HttpGet]
        public virtual NotifierSettingsModel Get(int activityType, int notificationType)
        {
            var activityEventIdentity = new ActivityEventIdentity(_activityTypeProvider.Get(activityType), _notificationTypeProvider.Get(notificationType));
            return _notificationSettingsService.GetSettings(activityEventIdentity);
        }

        [HttpPost]
        public virtual void SaveUiNotifierSetting(NotifierSettingSaveModel<UiNotifierTemplate> notifierSettingModel)
        {
            var mappedModel = notifierSettingModel.Map<NotifierSettingModel<UiNotifierTemplate>>();
            _notificationSettingsService.Save(mappedModel);
        }

        [HttpPost]
        public virtual void SaveEmailNotifierSetting(NotifierSettingSaveModel<EmailNotifierTemplate> notifierSettingModel)
        {
            var mappedModel = notifierSettingModel.Map<NotifierSettingModel<EmailNotifierTemplate>>();
            _notificationSettingsService.Save(mappedModel);
        }
    }
}