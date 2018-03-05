using System.Collections.Generic;

namespace Uintra.Notification.Configuration
{
    public interface INotificationSettingCategoryProvider
    {
        IEnumerable<NotificationSettingsCategoryDto> GetAvailableCategories();
    }
}