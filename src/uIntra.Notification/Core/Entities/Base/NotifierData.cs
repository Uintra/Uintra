using System;
using System.Collections.Generic;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Base
{
    public class NotifierData
    {
        public IEnumerable<Guid> ReceiverIds { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public INotifierDataValue Value { get; set; }
    }
}
