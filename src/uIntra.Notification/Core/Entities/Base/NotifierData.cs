using System;
using System.Collections.Generic;

namespace Uintra.Notification.Base
{
    public class NotifierData
    {
        public IEnumerable<Guid> ReceiverIds { get; set; }
        public Enum NotificationType { get; set; }
        public Enum ActivityType { get; set; }
        public INotifierDataValue Value { get; set; }
    }
}
