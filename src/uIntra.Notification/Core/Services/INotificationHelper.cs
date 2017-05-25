using Umbraco.Core.Models;

namespace uCommunity.Notification.Core.Services
{
    public interface INotificationHelper
    {
        IPublishedContent GetNotificationListPage();
    }
}
