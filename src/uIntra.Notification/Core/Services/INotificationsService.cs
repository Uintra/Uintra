using Uintra.Notification.Base;

namespace Uintra.Notification
{
    public interface INotificationsService
    {
        void ProcessNotification(NotifierData data);
    }
}
