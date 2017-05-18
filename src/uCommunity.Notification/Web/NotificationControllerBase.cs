using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using uCommunity.Core;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;
using uCommunity.Notification.Core.Models;
using uCommunity.Notification.Core.Services;
using Umbraco.Web.Mvc;

namespace uCommunity.Notification.Web
{
    public abstract class NotificationControllerBase: SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Notification/List/NotificationOverview.cshtml";
        protected virtual string ListViewPath { get; } = "~/App_Plugins/Notification/List/NotificationList.cshtml";
        protected virtual int ItemsPerPage { get; } = 8;

        protected readonly IUiNotifierService UiNotifierService;
        protected readonly IIntranetUserService<IIntranetUser> IntranetUserService;
        

        protected NotificationControllerBase(IUiNotifierService uiNotifierService, IIntranetUserService<IIntranetUser> intranetUserService)
        {
            UiNotifierService = uiNotifierService;
            IntranetUserService = intranetUserService;
        }

        public virtual ActionResult Overview()
        {
            return PartialView(OverviewViewPath);
        }

        public virtual ActionResult Index(int page = 1)
        {
            var take = page * ItemsPerPage;
            var userId = IntranetUserService.GetCurrentUserId();
            int totalCount;
            var notifications = UiNotifierService.GetByReceiver(userId, take, out totalCount).ToList();
            return PartialView(ListViewPath,
                new NotificationListViewModel
                {
                    Notifications = notifications.Map<IEnumerable<NotificationViewModel>>(),
                    BlockScrolling = totalCount <= take
                });
        }

        [System.Web.Mvc.HttpGet]
        public virtual int GetNotNotifiedCount()
        {
            var userId = IntranetUserService.GetCurrentUserId();
            var count = UiNotifierService.GetNotNotifiedCount(userId);
            return count;
        }

        [System.Web.Mvc.HttpPost]
        public virtual void View([FromBody]Guid id)
        {
            UiNotifierService.ViewNotification(id);
        }

        public virtual PartialViewResult Notifications(NotificationListViewModel notificationList)
        {
            return PartialView(ListViewPath, notificationList);
        }
    }
}
