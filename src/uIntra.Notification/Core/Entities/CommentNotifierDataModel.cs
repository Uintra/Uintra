using System;
using Uintra.Notification.Base;

namespace Uintra.Notification
{
    public class CommentNotifierDataModel: INotifierDataValue, IHaveNotifierId
    {
        public Enum NotificationType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public Guid NotifierId { get; set; }
        public Guid CommentId { get; set; }
    }
}
