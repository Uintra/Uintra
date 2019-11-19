namespace Uintra20.Features.Notification.Models
{
    public class DesktopNotificationMessage : INotificationMessage
    {
        public string Message { get; set; }
        public string Title { get; set; }
    }
}