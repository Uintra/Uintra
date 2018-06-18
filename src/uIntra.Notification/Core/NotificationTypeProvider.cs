using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;
using Uintra.Notification.Configuration;

namespace Uintra.Notification
{
    public class NotificationTypeProvider : EnumTypeProviderBase<NotificationTypeEnum>, INotificationTypeProvider
    {
        public IEnumerable<Enum> PopupNotificationTypes()
        {
            return new List<Enum> { NotificationTypeEnum.Welcome };
        }

        public IEnumerable<Enum> UiNotificationTypes()
        {
            return All.Except(base[NotificationTypeEnum.Welcome.ToInt()].ToEnumerable());
        }
    }
}
