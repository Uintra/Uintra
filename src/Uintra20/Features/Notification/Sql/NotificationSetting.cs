﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Persistence;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Notification.Sql
{
    [UintraTable("NotificationSetting")]
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