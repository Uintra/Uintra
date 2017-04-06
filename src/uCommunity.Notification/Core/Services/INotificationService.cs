using uCommunity.Notification.Core.Entities;

namespace uCommunity.Notification.Core.Services
{
    public interface INotificationService
    {
        void ProcessNotification(NotifierData data);
    }
}
