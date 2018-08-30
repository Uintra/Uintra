namespace Compent.Uintra.Core.Notification
{
    public interface IUserMentionNotificationService
    {
        void SendNotification(UserMentionNotificationModel model);
    }
}