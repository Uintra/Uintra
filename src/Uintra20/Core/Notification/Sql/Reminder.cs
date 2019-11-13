using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Core.Notification.Configuration;
using Uintra20.Persistence;

namespace Uintra20.Core.Notification.Sql
{
    [UintraTable("Reminder")]
    public class Reminder : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid ActivityId { get; set; }
        public bool IsDelivered { get; set; }
        public ReminderTypeEnum Type { get; set; }
    }
}