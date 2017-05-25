using System;
using System.Collections.Generic;
using uIntra.Notification.Core.Configuration;

namespace uIntra.Notification.Core.Entities.Base
{
    public class NotifierData
    {
        public IEnumerable<Guid> ReceiverIds { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public INotifierDataValue Value { get; set; }
    }
}
