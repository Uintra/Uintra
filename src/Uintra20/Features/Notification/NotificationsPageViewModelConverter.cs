using System;
using System.Linq;
using System.Threading.Tasks;
using UBaseline.Core.Node;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Notification.ViewModel;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Notification
{
	public class NotificationsPageViewModelConverter : INodeViewModelConverter<NotificationsPageModel, NotificationsPageViewModel>
	{
		private readonly INodeModelService _nodeModelService;
		private readonly IUBaselineRequestContext _requestContext;
		private readonly IUiNotificationService _uiNotifierService;
		private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

		public NotificationsPageViewModelConverter(
			INodeModelService nodeModelService,
			IUBaselineRequestContext requestContext,
			IUiNotificationService uiNotifierService,
			IIntranetMemberService<IntranetMember> intranetMemberService)
		{
			_nodeModelService = nodeModelService;
			_requestContext = requestContext;
			_uiNotifierService = uiNotifierService;
			_intranetMemberService = intranetMemberService;
		}
		public void Map(NotificationsPageModel node, NotificationsPageViewModel viewModel)
		{
			var itemsCountForPopup = _nodeModelService
										 .GetByAlias<NotificationsPageModel>("notificationPage", _requestContext.HomeNode.RootId)
										 ?.NotificationsPopUpCount
										 ?.Value ?? default(int);

			var (notifications, _) = _uiNotifierService.GetMany(_intranetMemberService.GetCurrentMemberId(), itemsCountForPopup);

			var notificationsArray = notifications.ToArray();

			var notNotifiedNotifications = notificationsArray
				.Where(el => !el.IsNotified)
				.ToArray();

			if (notNotifiedNotifications.Length > 0)
			{
				_uiNotifierService.Notify(notNotifiedNotifications);
			}

			var notificationsViewModels =
				notificationsArray
					.Take(itemsCountForPopup)
					.Select(MapNotificationToViewModel);

			viewModel.Notifications = notificationsViewModels.ToArray();
		}

		protected virtual NotificationViewModel MapNotificationToViewModel(Sql.Notification notification)
		{
			var result = notification.Map<NotificationViewModel>();

			var id = Guid.Parse((string)result.Value.notifierId);
			result.Notifier = _intranetMemberService.Get(id).Map<MemberViewModel>();
			return result;
		}
	}
}