using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Core.Links;
using uIntra.Core.Extentions;
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

        private readonly IUiNotifierService _uiNotifierService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotificationHelper _notificationHelper;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;

        protected NotificationControllerBase(
            IUiNotifierService uiNotifierService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationHelper notificationHelper,
            IIntranetUserContentHelper intranetUserContentHelper)
        {
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
            _notificationHelper = notificationHelper;
            _intranetUserContentHelper = intranetUserContentHelper;
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
            var notificationListPage = _notificationHelper.GetNotificationListPage();
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
                NotificationsUrl = _notificationHelper.GetNotificationListPage().Url
            };

            return PartialView(PreviewViewPath, result);
        }

        //TODO : move into helper
        protected virtual ProfileLink GetProfileLink(Guid memberId)
        {
            var profilePageUrl = _intranetUserContentHelper.GetProfilePage().Url;
            return new ProfileLink()
            {
                Value = profilePageUrl.AddIdParameter(memberId)
            };
        }

        protected NotifierViewModel GetNotifierViewModel(Guid notifierId)
        {
            var notifier = _intranetUserService.Get(notifierId);
            var result = new NotifierViewModel()
            {
                Id = notifierId,
                ProfileLink = GetProfileLink(notifierId),
                Name = notifier.DisplayedName,
                Photo = notifier.Photo
            };
            return result;
        }
    }
}
