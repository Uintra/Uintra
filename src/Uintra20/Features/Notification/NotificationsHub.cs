using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Features.Notification
{
	public class NotificationsHub : Hub
	{
		private readonly IUiNotificationService _uiNotificationService;

		public NotificationsHub(IUiNotificationService uiNotificationService)
		{
			_uiNotificationService = uiNotificationService;
		}

		public async Task<int> GetNotNotifiedCount(Guid currentMemberId)
		{
			return await _uiNotificationService.GetNotNotifiedCountAsync(currentMemberId);
		}
	}
}