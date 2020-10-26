using System;
using System.Collections.Generic;

namespace Uintra20.Features.Notification.Models
{
    public class UserMentionNotificationModel
    {
        public Guid MentionedSourceId { get; set; }
        public Guid CreatorId { get; set; }
        public IEnumerable<Guid> ReceivedIds { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Enum ActivityType { get; set; }
    }
}
