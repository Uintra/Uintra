using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Compent.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Uintra.Core;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Notification.Constants;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace Uintra.Notification.Web
{
    public abstract class NotificationControllerBase : SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Notification/List/NotificationOverview.cshtml";
        protected virtual string ListViewPath { get; } = "~/App_Plugins/Notification/List/NotificationList.cshtml";
        protected virtual string PreviewViewPath { get; } = "~/App_Plugins/Notification/List/NotificationPreview.cshtml";
        protected virtual string PopupNotificationsViewPath { get; } = "~/App_Plugins/Notification/List/PopupNotificationsView.cshtml";

        protected virtual int ItemsPerPage { get; } = 8;

        private readonly IUiNotificationService _uiNotifierService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotificationContentProvider _notificationContentProvider;
        private readonly IPopupNotificationService _popupNotificationService;

        protected NotificationControllerBase(
            IUiNotificationService uiNotifierService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationContentProvider notificationContentProvider,
            IProfileLinkProvider profileLinkProvider,
            IPopupNotificationService popupNotificationService)

        {
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
            _notificationContentProvider = notificationContentProvider;
            _popupNotificationService = popupNotificationService;
        }

        public virtual ActionResult Overview()
        {
            return PartialView(OverviewViewPath);
        }

        public virtual ActionResult Index(int page = 1)
        {
            var take = page * ItemsPerPage;
            var notifications = _uiNotifierService.GetMany(_intranetUserService.GetCurrentUserId(), take, out var totalCount).ToList();

            var notNotifiedNotifications = notifications.Where(el => !el.IsNotified).ToList();
            if (notNotifiedNotifications.Count > 0)
            {
                _uiNotifierService.Notify(notNotifiedNotifications);
            }

            var notificationsViewModels = notifications.Select(MapNotificationToViewModel).ToList();

            var result = new NotificationListViewModel
            {
                Notifications = notificationsViewModels,
                BlockScrolling = totalCount <= take
            };

            return PartialView(ListViewPath, result);
        }

        [System.Web.Mvc.HttpGet]
        public virtual int GetNotNotifiedCount()
        {
            var userId = _intranetUserService.GetCurrentUserId();
            var count = _uiNotifierService.GetNotNotifiedCount(userId);
            return count;
        }

        [System.Web.Mvc.HttpGet]
        public virtual ActionResult GetNotNotifiedNotifications()
        {
            var userId = _intranetUserService.GetCurrentUserId();

            var model = new JsonNotificationsModel();
            model.Notifications = _uiNotifierService.GetNotNotifiedNotifications(userId)
                .Select(MapNotificationToJsonModel);
            model.Count = model.Notifications.Count();
            return new JsonNetResult()
            {
                Data = model,
                SerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };
        }

        [System.Web.Mvc.AllowAnonymous]
        public ActionResult ShowPopupNotifications()
        {
            var receiverId = _intranetUserService.GetCurrentUserId();
            var notifications = _popupNotificationService.Get(receiverId).Map<IEnumerable<PopupNotificationViewModel>>();
            return PartialView(PopupNotificationsViewPath, notifications);
        }

        [System.Web.Mvc.HttpPost]
        public virtual void View([FromBody]Guid id)
        {
            _uiNotifierService.ViewNotification(id);
        }

        [System.Web.Mvc.HttpPost]
        public virtual void ViewPopup([FromBody]Guid id)
        {
            _popupNotificationService.ViewNotification(id);
        }

        public virtual PartialViewResult List()
        {
            var notificationListPage = _notificationContentProvider.GetNotificationListPage();
            var itemsCountForPopup = notificationListPage.GetPropertyValue(NotificationConstants.ItemCountForPopupPropertyTypeAlias, default(int));
            var notifications = _uiNotifierService.GetMany(_intranetUserService.GetCurrentUserId(), itemsCountForPopup, out _).ToList();

            var notNotifiedNotifications = notifications.Where(el => !el.IsNotified).ToList();
            if (notNotifiedNotifications.Count > 0)
            {
                _uiNotifierService.Notify(notNotifiedNotifications);
            }

            var notificationsViewModels = notifications.Select(MapNotificationToViewModel).ToList();

            var result = new NotificationListViewModel
            {
                Notifications = notificationsViewModels,
                BlockScrolling = false
            };

            return PartialView(ListViewPath, result);
        }

        private NotificationViewModel MapNotificationToViewModel(Notification notification)
        {
            var result = notification.Map<NotificationViewModel>();

            result.Notifier = ((string)result.Value.notifierId)
                .TryParseGuid()
                .FromNullable(_intranetUserService.Get);

            return result;
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

        public virtual PartialViewResult Preview()
        {
            var result = new NotificationPreviewViewModel
            {
                NotificationsUrl = _notificationContentProvider.GetNotificationListPage().Url
            };

            return PartialView(PreviewViewPath, result);
        }
    }
}
