using Umbraco.Core.Models;

namespace uIntra.Notification
{
    public interface INotificationContentProvider
    {
        IPublishedContent GetNotificationListPage();
    }
}
