using System;
using ServiceStack.DataAnnotations;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
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