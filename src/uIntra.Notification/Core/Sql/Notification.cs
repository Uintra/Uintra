using System;
using ServiceStack.DataAnnotations;
using uIntra.Core.Persistence.Sql;
using uIntra.Notification.Core.Configuration;

namespace uIntra.Notification.Core.Sql
{
    public class Notification : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime Date { get; set; }
        public bool IsNotified { get; set; }
        public bool IsViewed { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public string Value { get; set; }
    }
}