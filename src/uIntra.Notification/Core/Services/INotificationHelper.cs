using Umbraco.Core.Models;

namespace uIntra.Notification
{
    public interface INotificationHelper
    {
        IPublishedContent GetNotificationListPage();
    }
}
