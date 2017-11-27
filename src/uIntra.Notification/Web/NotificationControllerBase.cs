using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.User;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Notification.Web
{
    public abstract class NotificationControllerBase : SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Notification/List/NotificationOverview.cshtml";
        protected virtual string ListViewPath { get; } = "~/App_Plugins/Notification/List/NotificationList.cshtml";
        protected virtual string PreviewViewPath { get; } = "~/App_Plugins/Notification/List/NotificationPreview.cshtml";

        protected virtual int ItemsPerPage { get; } = 8;

        private readonly IUiNotificationService _uiNotifierService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotificationContentProvider _notificationContentProvider;
        private readonly IProfileLinkProvider _profileLinkProvider;

        protected NotificationControllerBase(
            IUiNotificationService uiNotifierService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationContentProvider notificationContentProvider,
            IProfileLinkProvider profileLinkProvider)

        {
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
            _notificationContentProvider = notificationContentProvider;
            _profileLinkProvider = profileLinkProvider;
        }

        public virtual ActionResult Overview()
        {
            return PartialView(OverviewViewPath);
        }

        public virtual ActionResult Index(int page = 1)
        {
            var take = page * ItemsPerPage;
            int totalCount;
            var notifications = _uiNotifierService.GetMany(_intranetUserService.GetCurrentUserId(), take, out totalCount).ToList();

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

        [System.Web.Mvc.HttpPost]
        public virtual void View([FromBody]Guid id)
        {
            _uiNotifierService.ViewNotification(id);
        }

        public virtual PartialViewResult List()
        {
            int totalCount;
            var notificationListPage = _notificationContentProvider.GetNotificationListPage();
            var itemsCountForPopup = notificationListPage.GetPropertyValue(NotificationConstants.ItemCountForPopupPropertyTypeAlias, default(int));
            var notifications = _uiNotifierService.GetMany(_intranetUserService.GetCurrentUserId(), itemsCountForPopup, out totalCount).ToList();

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

            Guid id;
            if (Guid.TryParse((string)result.Value.notifierId, out id))
                result.Notifier = GetNotifierViewModel(id);

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

        protected NotifierViewModel GetNotifierViewModel(Guid notifierId)
        {
            var notifier = _intranetUserService.Get(notifierId);
            var result = new NotifierViewModel()
            {
                Id = notifierId,
                ProfileLink = _profileLinkProvider.GetProfileLink(notifierId),
                Name = notifier.DisplayedName,
                Photo = notifier.Photo
            };
            return result;
        }
    }
}
