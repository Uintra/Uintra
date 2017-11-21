using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Activity;
using uIntra.Core.Persistence;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Core.Sql
{

    [uIntraTable("NotificationSetting")]
    public class NotificationSetting : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public int ActivityType { get; set; }
        public int NotificationType { get; set; }
        public int NotifierType { get; set; }
        public bool IsEnabled { get; set; }

        [StringLength(int.MaxValue)]
        public string JsonData { get; set; } 
    }
}
