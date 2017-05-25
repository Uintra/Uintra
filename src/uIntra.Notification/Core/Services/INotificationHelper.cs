using Umbraco.Core.Models;

namespace uIntra.Notification.Core.Services
{
    public interface INotificationHelper
    {
        IPublishedContent GetNotificationListPage();
    }
}
