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
            return _notificationSettingsService.Get(activityType, notificationType);
        }

        [HttpPost]
        public virtual void Save(NotifierSettingsModel settings)
        {
            _notificationSettingsService.Save(settings);
        }
    }
}