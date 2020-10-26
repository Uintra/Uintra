using System;
using System.Collections.Generic;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface INotificationTypeProvider : IEnumTypeProvider
    {
        IEnumerable<Enum> PopupNotificationTypes();

        IEnumerable<Enum> UiNotificationTypes();
    }
}
