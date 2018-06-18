using Umbraco.Core.Models;

namespace Uintra.Notification
{
    public interface INotificationContentProvider
    {
        IPublishedContent GetNotificationListPage();
    }
}
