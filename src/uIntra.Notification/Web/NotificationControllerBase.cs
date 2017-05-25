using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Core.User;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace uIntra.Notification.Web
{
    public abstract class NotificationControllerBase: SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Notification/List/NotificationOverview.cshtml";
        protected virtual string ListViewPath { get; } = "~/App_Plugins/Notification/List/NotificationList.cshtml";
        protected virtual int ItemsPerPage { get; } = 8;

        private readonly IUiNotifierService _uiNotifierService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        
        protected NotificationControllerBase(IUiNotifierService uiNotifierService, IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
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
            return PartialView(ListViewPath,
                new NotificationListViewModel
                {
                    Notifications = notifications.Map<IEnumerable<NotificationViewModel>>().ForEach(FillNotifierData),
                    BlockScrolling = totalCount <= take
                });
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

        public virtual PartialViewResult Notifications(NotificationListViewModel notificationList)
        {
            return PartialView(ListViewPath, notificationList);
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
