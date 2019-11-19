using System;
using Uintra20.Features.Notification.Entities.Base;

namespace Uintra20.Features.Notification.Entities
{
    public class GroupInvitationDataModel :
        INotifierDataValue,
        IHaveNotifierId
    {
        public Guid NotifierId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid GroupId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Enum NotificationType { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
    }
}