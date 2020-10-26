using System;
using Uintra.Features.Links.Models;
using Uintra.Features.Notification.Entities.Base;

namespace Uintra.Features.Notification.Entities
{
    public class ActivityReminderDataModel : INotifierDataValue
    {
        public Enum NotificationType { get; set; }
        public Enum ActivityType { get; set; }
        public string Title { get; set; }
        public UintraLinkModel Url { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public DateTime StartDate { get; set; }
    }
}