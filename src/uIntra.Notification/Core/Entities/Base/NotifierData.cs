using System;
using System.Collections.Generic;
using uCommunity.Notification.Core.Configuration;

namespace uCommunity.Notification.Core.Entities
{
    public class NotifierData
    {
        public IEnumerable<Guid> ReceiverIds { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public INotifierDataValue Value { get; set; }
    }
}
