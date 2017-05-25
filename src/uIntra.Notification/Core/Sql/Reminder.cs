using System;
using ServiceStack.DataAnnotations;
using uIntra.Core.Persistence.Sql;
using uIntra.Notification.Core.Configuration;

namespace uIntra.Notification.Core.Sql
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
