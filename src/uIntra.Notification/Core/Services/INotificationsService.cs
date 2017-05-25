using uIntra.Notification.Core.Entities.Base;

namespace uIntra.Notification.Core.Services
{
    public interface INotificationsService
    {
        void ProcessNotification(NotifierData data);
    }
}
