using System;
using System.Collections.Generic;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface INotificationTypeProvider : IEnumTypeProvider
    {
        IEnumerable<Enum> PopupNotificationTypes();

        IEnumerable<Enum> UiNotificationTypes();
    }
}
