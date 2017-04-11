using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.Persistence.Sql;
using uCommunity.Notification.Core.Configuration;

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
