using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Features.Notification.Configuration;
using Uintra.Persistence;
using Uintra.Persistence.Sql;

namespace Uintra.Features.Notification.Sql
{
    [UintraTable("Notification")]
    public class Notification : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid ReceiverId { get; set; }
        public DateTime Date { get; set; }
        public bool IsNotified { get; set; }
        public bool IsViewed { get; set; }
        public int Type { get; set; }
        [DefaultValue((int)NotifierTypeEnum.UiNotifier)]
        public int NotifierType { get; set; }
        public string Value { get; set; }
    }
}