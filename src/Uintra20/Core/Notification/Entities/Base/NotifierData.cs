using System;
using System.Collections.Generic;

namespace Uintra20.Core.Notification.Base
{
    public class NotifierData
    {
        public IEnumerable<Guid> ReceiverIds { get; set; }
        public Enum NotificationType { get; set; }
        public Enum ActivityType { get; set; }
        public INotifierDataValue Value { get; set; }
    }
}