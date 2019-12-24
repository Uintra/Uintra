using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<NotificationListViewModel> Get(int page = 1)
        {
            var take = page * _itemsPerPage;
            var (notifications, totalCount) = _uiNotifierService.GetMany( await _intranetMemberService.GetCurrentMemberIdAsync(), take);

            var notificationsArray = notifications.ToArray();

            var notNotifiedNotifications = notificationsArray.Where(el => !el.IsNotified).ToArray();
            if (notNotifiedNotifications.Length > 0)
            {
                await _uiNotifierService.NotifyAsync(notNotifiedNotifications);
            }

            var notificationsViewModels = await Task.WhenAll(
                notificationsArray
                    .Select(async n => await MapNotificationToViewModelAsync(n)));

            var result = new NotificationListViewModel
            {
                Notifications = notificationsViewModels,
                BlockScrolling = totalCount <= take
            };

            return result;
        }

        [HttpGet]
        public async Task<NotificationListViewModel> NotificationList()
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
                await _uiNotifierService.NotifyAsync(notNotifiedNotifications);
            }

            var notificationsViewModels = await Task.WhenAll(
                notificationsArray
                    .Take(itemsCountForPopup)
                    .Select(async n => await MapNotificationToViewModelAsync(n)));

            return new NotificationListViewModel
            {
                Notifications = notificationsViewModels,
                BlockScrolling = false
            };
        }

        [HttpGet]
        public async Task<IEnumerable<PopupNotificationViewModel>> GetPopupNotifications()
        {
            var receiverId = await _intranetMemberService.GetCurrentMemberIdAsync();
            var notifications = (await _popupNotificationService.GetAsync(receiverId))
                .Map<IEnumerable<PopupNotificationViewModel>>();

            return notifications;
        }

        [HttpGet]
        public async Task<int> GetNotNotifiedCount()
        {
            var currentMember = await _intranetMemberService.GetCurrentMemberAsync();

            var count = currentMember != null
                ? await _uiNotifierService.GetNotNotifiedCountAsync(currentMember.Id)
                : default(int);

            return count;
        }

        [HttpPost]
        public async Task UpdateNotifierSettings(NotifierTypeEnum type, bool isEnabled)
        {
            var currentMember = await _intranetMemberService.GetCurrentMemberAsync();
            await _memberNotifiersSettingsService.SetForMemberAsync(currentMember.Id, type, isEnabled);
        }

        [HttpPost]
        public Task SetNotificationViewed([FromBody]Guid id)
        {
            return _uiNotifierService.ViewNotificationAsync(id);
        }

        [HttpPost]
        public Task SetPopupNotificationViewed([FromBody]Guid id)
        {
            return _popupNotificationService.ViewNotificationAsync(id);
        }

        private async Task<NotificationViewModel> MapNotificationToViewModelAsync(Sql.Notification notification)
        {
            var result = notification.Map<NotificationViewModel>();

            var notifierId = (string)result.Value.notifierId;

            result.Notifier = Guid.TryParse(notifierId, out var id)
                ? (await _intranetMemberService.GetAsync(id)).Map<MemberViewModel>()
                : null;

            return result;
        }

    }
}