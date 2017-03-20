namespace uCommunity.Notification.Notifier
{
    public interface INotificationService
    {
        void ProcessNotification(NotifierData data);
    }
}
