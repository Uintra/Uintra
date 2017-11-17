using uIntra.Core.Activity;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Models;
using uIntra.Notification.Core.Services;
using Umbraco.Web.WebApi;
using System.Web.Http;

namespace uIntra.Notification.Web
{
    public abstract class NotificationSettingsApiControllerBase : UmbracoAuthorizedApiController
    {
        private readonly INotificationSettingsService _notificationSettingsService;

        protected NotificationSettingsApiControllerBase(INotificationSettingsService notificationSettingsService)
        {
            _notificationSettingsService = notificationSettingsService;
        }

        [HttpGet]
        public virtual NotifierSettingsModel Get(IntranetActivityTypeEnum activityType, NotificationTypeEnum notificationType)
        {
            var activityEventIdentity = new ActivityEventIdentity(activityType, notificationType);
            return _notificationSettingsService.Get(activityEventIdentity);
        }

        [HttpPost]
        public virtual void SaveUiNotifierSetting(NotifierSettingModel<UiNotifierTemplate> notifierSettingModel)
        {
            _notificationSettingsService.Save(notifierSettingModel);
        }

        [HttpPost]
        public virtual void SaveEmailNotifierSetting(NotifierSettingModel<EmailNotifierTemplate> notifierSettingModel)
        {
            _notificationSettingsService.Save(notifierSettingModel);
        }
    }
}