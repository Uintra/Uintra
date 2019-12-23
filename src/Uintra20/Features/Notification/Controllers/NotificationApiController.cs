using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Notification.Settings;
using Uintra20.Features.Notification.ViewModel;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Notification.Controllers
{
    public class NotificationApiController : UBaselineApiController
    {
        private readonly int _itemsPerPage;
        private readonly IUBaselineRequestContext _requestContext;
        private readonly INodeModelService _nodeModelService;
        private readonly IUiNotificationService _uiNotifierService;
        private readonly IPopupNotificationService _popupNotificationService;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        public NotificationApiController(
            NotificationSettings notificationSettings,
            IUBaselineRequestContext requestContext,
            INodeModelService nodeModelService,
            IUiNotificationService uiNotifierService,
            IPopupNotificationService popupNotificationService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _itemsPerPage = notificationSettings.ItemsPerPage;
            _requestContext = requestContext;
            _nodeModelService = nodeModelService;
            _uiNotifierService = uiNotifierService;
            _popupNotificationService = popupNotificationService;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _intranetMemberService = intranetMemberService;
        }

        [HttpGet]
        public NotificationListViewModel Get(int page = 1)
        {
            var take = page * _itemsPerPage;
            var (notifications, totalCount) = _uiNotifierService.GetMany(_intranetMemberService.GetCurrentMemberId(), take);

            var notificationsArray = notifications.ToArray();

            var notNotifiedNotifications = notificationsArray.Where(el => !el.IsNotified).ToArray();
            if (notNotifiedNotifications.Length > 0)
            {
                _uiNotifierService.Notify(notNotifiedNotifications);
            }

            var notificationsViewModels = notificationsArray
                .Select(MapNotificationToViewModel)
                .ToArray();

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
                .GetByAlias<NotificationPageModel>("notificationPage", _requestContext.HomeNode.RootId)
                ?.NotificationsPopUpCount
                ?.Value ?? default(int);

            var (notifications, _) = _uiNotifierService
                .GetMany(_intranetMemberService.GetCurrentMemberId(), itemsCountForPopup);

            var notificationsArray = notifications.ToArray();

            var notNotifiedNotifications = notificationsArray
                .Where(el => !el.IsNotified)
                .ToArray();

            if (notNotifiedNotifications.Length > 0)
            {
                _uiNotifierService.Notify(notNotifiedNotifications);
            }

            var notificationsViewModels = notificationsArray
                .Take(itemsCountForPopup)
                .Select(MapNotificationToViewModel)
                .ToArray();

            return new NotificationListViewModel
            {
                Notifications = notificationsViewModels,
                BlockScrolling = false
            };
        }

        [HttpGet]
        public IEnumerable<PopupNotificationViewModel> GetPopupNotifications()
        {
            var receiverId = _intranetMemberService.GetCurrentMemberId();
            var notifications = _popupNotificationService.Get(receiverId).Map<IEnumerable<PopupNotificationViewModel>>();
            return notifications;
        }

        [HttpGet]
        public int GetNotNotifiedCount()
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            var count = currentMember != null
                ? _uiNotifierService.GetNotNotifiedCount(currentMember.Id)
                : default(int);

            return count;
        }

        [HttpPost]
        public void UpdateNotifierSettings(NotifierTypeEnum type, bool isEnabled)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            _memberNotifiersSettingsService.SetForMember(currentMember.Id, type, isEnabled);
        }

        [HttpPost]
        public virtual void SetNotificationViewed([FromBody]Guid id)
        {
            _uiNotifierService.ViewNotification(id);
        }

        [HttpPost]
        public virtual void SetPopupNotificationViewed([FromBody]Guid id)
        {
            _popupNotificationService.ViewNotification(id);
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