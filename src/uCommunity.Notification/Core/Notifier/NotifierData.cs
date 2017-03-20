using System;
using System.Collections.Generic;

namespace uCommunity.Notification.Notifier
{
    public class NotifierData
    {
        public IEnumerable<Guid> ReceiverIds { get; set; }

        public NotificationTypeEnum NotificationType { get; set; }

        public INotifierDataValue Value { get; set; }
    }
}
