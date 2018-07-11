using Compent.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Notification.Models.Json;
using Umbraco.Web.WebApi;

namespace Uintra.Notification.Web
{
    public class DesktopNotificationController : UmbracoApiController
    {
        private readonly IUiNotificationService _uiNotifierService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotificationContentProvider _notificationContentProvider;
        private readonly IPopupNotificationService _popupNotificationService;
        private readonly INotificationSettingsService _notificationSettingsService;

        public DesktopNotificationController(
            IUiNotificationService uiNotifierService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationContentProvider notificationContentProvider,
            IProfileLinkProvider profileLinkProvider,
            IPopupNotificationService popupNotificationService,
            INotificationSettingsService notificationSettingsService)

        {
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
            _notificationContentProvider = notificationContentProvider;
            _popupNotificationService = popupNotificationService;
            _notificationSettingsService = notificationSettingsService;
        }

        [HttpGet]
        public JsonNotificationsModel Get()
        {
            var userId = _intranetUserService.GetCurrentUserId();

            var notNotifiedNotifications = _uiNotifierService.GetNotNotifiedNotifications(userId);
            var count = notNotifiedNotifications.Count();
            if (count > 0)
                _uiNotifierService.Notify(notNotifiedNotifications);

            var model = new JsonNotificationsModel()
            {
                Count = count,
                Notifications = notNotifiedNotifications.Select(MapNotificationToJsonModel),
            };
            return model;
        }

        [HttpPost]
        public IHttpActionResult Viewed(Guid id)
        {
            _uiNotifierService.ViewNotification(id);
            return this.Ok();
        }

        public void Config()
        {
            _notificationSettingsService.get;
        }

        private JsonNotification MapNotificationToJsonModel(Notification notification)
        {
            var result = notification.Map<JsonNotification>();
            var notifier = ((string)result.Value.notifierId)
                .TryParseGuid()
                .FromNullable(_intranetUserService.Get);
            result.NotifierId = notifier.Id;
            result.NotifierPhoto = notifier.Photo;
            result.NotifierDisplayedName = notifier.DisplayedName;
            return result;
        }
    }
}
