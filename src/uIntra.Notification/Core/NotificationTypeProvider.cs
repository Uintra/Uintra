using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Extensions;
using Uintra.Core.TypeProviders;
using Uintra.Notification.Configuration;
using static LanguageExt.Prelude;

namespace Uintra.Notification
{
    public class NotificationTypeProvider : EnumTypeProviderBase<NotificationTypeEnum>, INotificationTypeProvider
    {
        public IEnumerable<Enum> PopupNotificationTypes() => 
            List<Enum>(NotificationTypeEnum.Welcome);

        public IEnumerable<Enum> UiNotificationTypes() => 
            All.Except(List(base[NotificationTypeEnum.Welcome.ToInt()]));
    }
}
