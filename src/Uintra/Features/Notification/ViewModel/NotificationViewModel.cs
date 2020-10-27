﻿using System;
using Uintra.Core.Member.Models;

namespace Uintra.Features.Notification.ViewModel
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public bool IsNotified { get; set; }
        public bool IsViewed { get; set; }
        public Enum Type { get; set; }
        public MemberViewModel Notifier { get; set; }
        public dynamic Value { get; set; }
    }
}