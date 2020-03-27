using Uintra20.Features.Notification.Models;

namespace Uintra20.Features.Notification.Services
{
    public interface IUserMentionNotificationService
    {
        void SendNotification(UserMentionNotificationModel model);
    }
}
