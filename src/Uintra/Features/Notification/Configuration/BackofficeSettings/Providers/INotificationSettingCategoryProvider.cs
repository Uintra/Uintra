using System.Collections.Generic;

namespace Uintra.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface INotificationSettingCategoryProvider
    {
        IEnumerable<NotificationSettingsCategoryDto> GetAvailableCategories();
    }
}