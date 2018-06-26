using System;
using System.Collections.Generic;

namespace Compent.Uintra.Core.Notification
{
    public class UserMentionNotificationModel
    {
        public Guid CreatorId { get; set; }
        public IEnumerable<Guid> ReceivedIds { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}