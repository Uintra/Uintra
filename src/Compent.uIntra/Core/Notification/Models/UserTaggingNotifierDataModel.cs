using System;
using Uintra.Notification.Base;

namespace Compent.Uintra.Core.Notification
{
    public class UserTaggingNotifierDataModel : INotifierDataValue, IHaveNotifierId
    {
        public Enum NotificationType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public Guid NotifierId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}