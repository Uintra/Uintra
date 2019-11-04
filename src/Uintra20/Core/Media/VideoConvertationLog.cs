﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Persistence;

namespace Uintra20.Core.Media
{
    [UintraTable("VideoConvertationLog")]
    public class VideoConvertationLog : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public int MediaId { get; set; }
        public DateTime Date { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}