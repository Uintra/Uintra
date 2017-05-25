using System;
using ServiceStack.DataAnnotations;
using uCommunity.Notification.Core.Configuration;
using uIntra.Core.Persistence.Sql;

namespace uCommunity.Notification.Core.Sql
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
