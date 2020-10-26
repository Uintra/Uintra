using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Features.Notification.Configuration;
using Uintra.Persistence;
using Uintra.Persistence.Sql;

namespace Uintra.Features.Notification.Sql
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