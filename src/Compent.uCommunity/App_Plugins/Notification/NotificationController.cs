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

namespace uCommunity.Notification
{
    public class NotificationController : SurfaceController
    {
        private readonly IUiNotifierService _uiNotifierService;
        private readonly IIntranetUserService _intranetUserService;
        private int ItemsPerPage = 8;

        public NotificationController(IUiNotifierService uiNotifierService, IIntranetUserService intranetUserService)
        {
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
        }

        public ActionResult Overview()
        {
            return PartialView("~/App_Plugins/Notification/List/NotificationOverview.cshtml");
        }

        public ActionResult Index(int page = 1)
        {
            var take = page * ItemsPerPage;
            var userId = _intranetUserService.GetCurrentUserId();
            int totalCount;
            var notifications = _uiNotifierService.GetByReceiver(userId, take, out totalCount).ToList();
            return PartialView("~/App_Plugins/Notification/List/NotificationList.cshtml",
                new NotificationListViewModel
                {
                    Notifications = notifications.Map<IEnumerable<NotificationViewModel>>(),
                    BlockScrolling = totalCount <= take
                });
        }

        [System.Web.Mvc.HttpGet]
        public int GetNotNotifiedCount()
        {
            var userId = _intranetUserService.GetCurrentUserId();
            var count = _uiNotifierService.GetNotNotifiedCount(userId);
            return count;
        }

        [System.Web.Mvc.HttpPost]
        public void View([FromBody]Guid id)
        {
            _uiNotifierService.ViewNotification(id);
        }
    }
}
