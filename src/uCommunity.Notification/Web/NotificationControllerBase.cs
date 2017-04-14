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
        public virtual string OverviewViewPath { get; } = "~/App_Plugins/Notification/List/NotificationOverview.cshtml";
        public virtual string ListViewPath { get; } = "~/App_Plugins/Notification/List/NotificationList.cshtml";

        protected readonly IUiNotifierService _uiNotifierService;
        protected readonly IIntranetUserService _intranetUserService;
        protected virtual int ItemsPerPage { get; } = 8;

        protected NotificationControllerBase(IUiNotifierService uiNotifierService, IIntranetUserService intranetUserService)
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
            var notifications = _uiNotifierService.GetByReceiver(userId, take, out totalCount).ToList();
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
            var userId = _intranetUserService.GetCurrentUserId();
            var count = _uiNotifierService.GetNotNotifiedCount(userId);
            return count;
        }

        [System.Web.Mvc.HttpPost]
        public virtual void View([FromBody]Guid id)
        {
            _uiNotifierService.ViewNotification(id);
        }
    }
}
