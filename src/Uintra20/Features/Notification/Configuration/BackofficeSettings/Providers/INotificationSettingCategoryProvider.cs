using System.Collections.Generic;

namespace Uintra20.Features.Notification.Configuration.BackofficeSettings.Providers
{
    public interface INotificationSettingCategoryProvider
    {
        IEnumerable<NotificationSettingsCategoryDto> GetAvailableCategories();
    }
}