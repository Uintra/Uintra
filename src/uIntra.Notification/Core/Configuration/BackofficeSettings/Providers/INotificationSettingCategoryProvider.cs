using System.Collections.Generic;

namespace uIntra.Notification.Configuration
{
    public interface INotificationSettingCategoryProvider
    {
        IEnumerable<NotificationSettingsCategoryDto> GetAvailableCategories();
    }
}