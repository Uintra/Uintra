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
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Notification.ViewModel;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Notification.Controllers
{
    public class NotificationApiController : UBaselineApiController
    {
        private int ItemsPerPage { get; } = 10;

        private readonly IUBaselineRequestContext _requestContext;
        private readonly INodeModelService _nodeModelService;
        private readonly IUiNotificationService _uiNotifierService;
        private readonly IPopupNotificationService _popupNotificationService;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        public NotificationApiController(
            IUBaselineRequestContext requestContext,
            INodeModelService nodeModelService,
            IUiNotificationService uiNotifierService,
            IPopupNotificationService popupNotificationService,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _requestContext = requestContext;
            _nodeModelService = nodeModelService;
            _uiNotifierService = uiNotifierService;
            _popupNotificationService = popupNotificationService;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _intranetMemberService = intranetMemberService;
        }

        [HttpGet]
        public async Task<NotificationViewModel[]> Get(int page = 1)
        {
            var skip = (page - 1) * ItemsPerPage;

            var notifications =
                (await _uiNotifierService.GetManyAsync(
                    await _intranetMemberService.GetCurrentMemberIdAsync()))
                    .Skip(skip)
                    .Take(ItemsPerPage)
                .ToArray();

            var notNotifiedNotifications = notifications.Where(el => !el.IsNotified).ToArray();
            if (notNotifiedNotifications.Any())
            {
                await _uiNotifierService.NotifyAsync(notNotifiedNotifications);
            }

            return await Task.WhenAll(notifications.Select(async n => await MapNotificationToViewModelAsync(n)));
        }

        [HttpGet]
        public async Task<NotificationListViewModel> NotificationList()
        {
            var notificationPageModel = _nodeModelService
                .GetByAlias<NotificationsPageModel>("notificationsPage", _requestContext.HomeNode.RootId);

            var itemsCountForPopup = notificationPageModel
                                         ?.NotificationsPopUpCount
                                         ?.Value ?? default(int);

            var notifications =
                (await _uiNotifierService.GetManyAsync(
                    await _intranetMemberService.GetCurrentMemberIdAsync()))
                        .Take(itemsCountForPopup)
                .ToArray();


            var notNotifiedNotifications = notifications
                .Where(el => !el.IsNotified);

            if (notifications.Length > 0)
                await _uiNotifierService.NotifyAsync(notNotifiedNotifications);


            var notificationsViewModels = await Task.WhenAll(
                notifications
                    .Take(itemsCountForPopup)
                    .Select(async n => await MapNotificationToViewModelAsync(n)));

            return new NotificationListViewModel
            {
                NotificationPageUrl = notificationPageModel?.Url ?? string.Empty,
                Notifications = notificationsViewModels
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
        public Task SetNotificationViewed([FromBody]Guid id)
        {
            return _uiNotifierService.ViewNotificationAsync(id);
        }

        [HttpPost]
        public Task SetPopupNotificationViewed([FromBody]Guid id)
        {
            return _popupNotificationService.ViewNotificationAsync(id);
        }

        [HttpPost]
        public Task<bool> Notified(Guid id)
        {
            return _uiNotifierService.SetNotificationAsNotifiedAsync(id);
        }

        [HttpPost]
        public Task Viewed(Guid id)
        {
           return _uiNotifierService.ViewNotificationAsync(id);
        }
        public Task ViewPopup([FromUri]Guid id)
        {
            return _popupNotificationService.ViewNotificationAsync(id);
        }

        [HttpPost]
        public Task View(Guid id)
        {
            return _uiNotifierService.ViewNotificationAsync(id);
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