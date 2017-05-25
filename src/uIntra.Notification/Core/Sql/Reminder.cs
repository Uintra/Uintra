using System;
using ServiceStack.DataAnnotations;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public class Reminder: SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public bool IsDelivered { get; set; }
        public ReminderTypeEnum Type { get; set; }
    }
}
