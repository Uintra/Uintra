using System;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Notification.ViewModel;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Notification.Controllers
{
    public class NotificationApiController : UBaselineApiController
    {
        private const int ItemsPerPage = 8;

        private readonly IUBaselineRequestContext _requestContext;
        private readonly INodeModelService _nodeModelService;
        private readonly IUiNotificationService _uiNotifierService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        public NotificationApiController(
            IUBaselineRequestContext requestContext,
            INodeModelService nodeModelService,
            IUiNotificationService uiNotifierService,
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _requestContext = requestContext;
            _nodeModelService = nodeModelService;
            _uiNotifierService = uiNotifierService;
            _intranetMemberService = intranetMemberService;
        }

        [HttpGet]
        public NotificationListViewModel Get(int page = 1)
        {
            var take = page * ItemsPerPage;
            var (notifications, totalCount) = _uiNotifierService.GetMany(_intranetMemberService.GetCurrentMemberId(), take);

            var notificationsArray = notifications.ToArray();

            var notNotifiedNotifications = notificationsArray.Where(el => !el.IsNotified).ToArray();
            if (notNotifiedNotifications.Length > 0)
            {
                _uiNotifierService.Notify(notNotifiedNotifications);
            }

            var notificationsViewModels = notificationsArray.Select(MapNotificationToViewModel).ToArray();

            var result = new NotificationListViewModel
            {
                Notifications = notificationsViewModels,
                BlockScrolling = totalCount <= take
            };

            return result;
        }
        
        [HttpGet]
        public NotificationListViewModel NotificationList()
        {
            var itemsCountForPopup = _nodeModelService
                .GetByAlias<NotificationPageModel>("notificationPage",_requestContext.HomeNode.RootId)
                ?.NotificationsPopUpCount
                ?.Value ?? default(int);
            var (notifications, _) = _uiNotifierService.GetMany(_intranetMemberService.GetCurrentMemberId(), itemsCountForPopup);

            var notificationsArray = notifications.ToArray();

            var notNotifiedNotifications = notificationsArray.Where(el => !el.IsNotified).ToArray();
            if (notNotifiedNotifications.Length > 0)
            {
                _uiNotifierService.Notify(notNotifiedNotifications);
            }

            var notificationsViewModels = notificationsArray.Take(itemsCountForPopup).Select(MapNotificationToViewModel).ToArray();

           return new NotificationListViewModel
            {
                Notifications = notificationsViewModels,
                BlockScrolling = false
            };
        }

        private NotificationViewModel MapNotificationToViewModel(Sql.Notification notification)
        {
            var result = notification.Map<NotificationViewModel>();

            var notifierId = (string)result.Value.notifierId;

            result.Notifier = Guid.TryParse(notifierId, out var id)
                ? _intranetMemberService.Get(id).Map<MemberViewModel>()
                : null;

            return result;
        }

    }
}