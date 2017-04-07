using uCommunity.Notification.Core.Entities;

namespace uCommunity.Notification.Core.Services
{
    public interface INotificationsService
    {
        void ProcessNotification(NotifierData data);
    }
}
