namespace Compent.Uintra.Core.Notification
{
    public interface IUserTaggingNotificationService
    {
        void SendNotification(UserTaggingNotificationModel model);
    }
}