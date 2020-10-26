using Uintra.Features.Notification.Models;

namespace Uintra.Features.Notification.Services
{
    public interface IUserMentionNotificationService
    {
        void SendNotification(UserMentionNotificationModel model);
    }
}
