﻿using System;

namespace Uintra.Features.Subscribe.Models
{
    public class ActivitySubscribeSettingDto
    {
        public Guid ActivityId { get; set; }

        public bool CanSubscribe { get; set; }

        public string SubscribeNotes { get; set; }
    }
}