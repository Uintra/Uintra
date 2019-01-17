using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Compent.Extensions;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Notification.Constants;
using Uintra.Notification.Models.Json;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using static LanguageExt.Prelude;

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

        public virtual PartialViewResult Preview()
        {
            var result = new NotificationPreviewViewModel
            {
                NotificationsUrl = _notificationContentProvider.GetNotificationListPage().Url
            };

            return PartialView(PreviewViewPath, result);
        }


        [System.Web.Mvc.HttpGet]
        public virtual int GetNotNotifiedCount()
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            var count = currentUser != null
                ? _uiNotifierService.GetNotNotifiedCount(currentUser.Id)
                : default;

            return count;
        }

        [System.Web.Mvc.HttpGet]
        public virtual ActionResult GetNotNotifiedNotifications()
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            var notNotifiedNotifications = (currentUser != null
                ? _uiNotifierService.GetNotNotifiedNotifications(currentUser.Id)
                : Enumerable.Empty<Notification>())
                .ToList();

            var model = new JsonNotificationsModel
            {
                Count = notNotifiedNotifications.Count,
                Notifications = notNotifiedNotifications.Select(MapNotificationToJsonModel),
            };
            return new JsonNetResult
            {
                Data = model,
                SerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };
        }

        [System.Web.Mvc.HttpPost]
        public virtual void Viewed(Guid id)
        {
            _uiNotifierService.ViewNotification(id);
        }

        [System.Web.Mvc.HttpPost]
        public virtual void Notified(Guid id)
        {
            _uiNotifierService.SetNotificationAsNotified(id);
        }

        protected virtual NotificationViewModel MapNotificationToViewModel(Notification notification)
        {
            var result = notification.Map<NotificationViewModel>();

            result.Notifier = ((string) result.Value.notifierId)
                .Apply(parseGuid)
                .Map(id => _intranetUserService.Get(id).Map<UserViewModel>())
                .IfNone((UserViewModel) null);

            return result;
        }

        protected virtual JsonNotification MapNotificationToJsonModel(Notification notification)
        {
            var result = notification.Map<JsonNotification>();
            var notifier = ((string)result.Value.notifierId)
                .Apply(parseGuid)
                .Map(id => _intranetUserService.Get(id));

            notifier.IfSome(user =>
            {
                result.NotifierId = user.Id;
                result.NotifierPhoto = user.Photo;
                result.NotifierDisplayedName = user.DisplayedName;
            });
            
            result.IsDesktopNotificationEnabled &= !(Request.IsMobileBrowser() || Request.Browser.IsMobileDevice);
            return result;
        }
    }
}
