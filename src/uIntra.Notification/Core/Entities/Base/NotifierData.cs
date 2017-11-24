using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.Notification.Base
{
    public class NotifierData
    {
        public IEnumerable<Guid> ReceiverIds { get; set; }
        public IIntranetType NotificationType { get; set; }
        public IIntranetType ActivityType { get; set; }
        public INotifierDataValue Value { get; set; }
    }
}
