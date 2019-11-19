using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Persistence;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Notification.Sql
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