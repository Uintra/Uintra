using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Infrastructure.Extensions;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Features.Notification.Configuration.BackofficeSettings.Providers
{
	public class NotificationTypeProvider : EnumTypeProviderBase, INotificationTypeProvider
	{
		public NotificationTypeProvider(params Type[] enums) : base(enums)
		{

		}
		public IEnumerable<Enum> PopupNotificationTypes() => new List<Enum>() { NotificationTypeEnum.Welcome };
		public IEnumerable<Enum> UiNotificationTypes() => All.Except(base[NotificationTypeEnum.Welcome.ToInt()].ToListOfOne());

	}
}

