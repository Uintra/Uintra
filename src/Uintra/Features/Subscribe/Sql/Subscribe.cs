﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Persistence;
using Uintra.Persistence.Sql;

namespace Uintra.Features.Subscribe.Sql
{
    [UintraTable("Subscribe")]
    public class Subscribe : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Required]
        [Index("UQ_Subscribe_UserId_ActivityId", 1, IsUnique = true)]
        public Guid UserId { get; set; }

        [Required]
        [Index("UQ_Subscribe_UserId_ActivityId", 2, IsUnique = true)]
        public Guid ActivityId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public bool IsNotificationDisabled { get; set; }
    }
}