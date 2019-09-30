﻿using System;
using Uintra.Notification.Base;

namespace Uintra.Notification.Core.Entities
{
    public class GroupInvitationDataModel :
        INotifierDataValue,
        IHaveNotifierId
    {
        public Guid NotifierId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid GroupId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Enum NotificationType { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
    }
}
