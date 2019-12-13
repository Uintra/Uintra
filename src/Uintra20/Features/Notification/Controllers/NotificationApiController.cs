using System;
using Compent.Shared.Extensions;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Notification.ViewModel;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Notification.Controllers
{
    public class NotificationApiController : UBaselineApiController
    {
        private const int ItemsPerPage = 8;

        private readonly IUiNotificationService _uiNotifierService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly INotificationContentProvider _notificationContentProvider;
        private readonly IPopupNotificationService _popupNotificationService;
        public NotificationApiController(
            IUiNotificationService uiNotifierService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            INotificationContentProvider notificationContentProvider,
            IPopupNotificationService popupNotificationService)
        {
            _uiNotifierService = uiNotifierService;
            _intranetMemberService = intranetMemberService;
            _notificationContentProvider = notificationContentProvider;
            _popupNotificationService = popupNotificationService;
        }

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