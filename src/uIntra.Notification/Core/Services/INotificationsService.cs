using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public interface INotificationsService
    {
        void ProcessNotification(NotifierData data);
    }
}
