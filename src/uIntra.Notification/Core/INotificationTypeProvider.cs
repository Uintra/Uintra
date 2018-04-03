using System;
using System.Collections.Generic;
using Uintra.Core.TypeProviders;

namespace Uintra.Notification
{
    public interface INotificationTypeProvider : IEnumTypeProvider
    {
        IEnumerable<Enum> PopupNotificationTypes();

        IEnumerable<Enum> UiNotificationTypes();
    }
}
