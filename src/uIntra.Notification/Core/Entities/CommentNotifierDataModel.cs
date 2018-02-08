﻿using System;
using uIntra.Notification.Base;

namespace uIntra.Notification
{
    public class CommentNotifierDataModel: INotifierDataValue, IHaveNotifierId
    {
        public Enum NotificationType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsPinned { get; set; }
        public bool IsPinActual { get; set; }
        public Guid NotifierId { get; set; }
        public Guid CommentId { get; set; }
    }
}
