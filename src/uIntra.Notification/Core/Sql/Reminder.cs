using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;
using Uintra.Notification.Configuration;

namespace Uintra.Notification
{
    [UintraTable("Reminder")]
    public class Reminder: SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid ActivityId { get; set; }
        public bool IsDelivered { get; set; }
        public ReminderTypeEnum Type { get; set; }
    }
}
