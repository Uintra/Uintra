﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;
using Uintra.Notification.Configuration;

namespace Uintra.Notification
{
    [UintraTable("Notification")]
    public class Notification : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid ActivityId { get; set; }
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