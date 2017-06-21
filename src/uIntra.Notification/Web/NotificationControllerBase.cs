using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Core;
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

        protected NotificationControllerBase(IUiNotifierService uiNotifierService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationHelper notificationHelper)
        {
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
            _notificationHelper = notificationHelper;
        }

        public virtual ActionResult Overview()
        {
            return PartialView(OverviewViewPath);
        }

        public virtual ActionResult Index(int page = 1)
        {
            var take = page * ItemsPerPage;
            var userId = _intranetUserService.GetCurrentUserId();
            int totalCount;
            var notifications = _uiNotifierService.GetMany(userId, take, out totalCount).ToList();

            var result = new NotificationListViewModel
            {
                Notifications = notifications.Map<IEnumerable<NotificationViewModel>>().ForEach(FillNotifierData),
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
            var notifications = _uiNotifierService.GetMany(_intranetUserService.GetCurrentUserId(), itemsCountForPopup, out totalCount);

            var result = new NotificationListViewModel
            {
                Notifications = notifications.Map<IEnumerable<NotificationViewModel>>(),
                BlockScrolling = false
            };

            return PartialView(ListViewPath, result);
        }

        public virtual PartialViewResult Preview()
        {
            var result = new NotificationPreviewViewModel
            {
                NotificationsUrl = _notificationHelper.GetNotificationListPage().Url
            };

            return PartialView(PreviewViewPath, result);
        }

        #region utils

        private void FillNotifierData(NotificationViewModel notification)
        {
            Guid notifierId;
            if (!Guid.TryParse((string)notification.Value.notifierId, out notifierId))
            {
                return;
            }

            var notifier = _intranetUserService.Get(notifierId);
            notification.NotifierName = notifier.DisplayedName;
            notification.NotifierPhoto = notifier.Photo;
        }

        #endregion
    }
}
