using System;
using System.Collections.Generic;

namespace Uintra20.Features.User.Models
{
    public class MentionModel
    {
        public Guid MentionedSourceId { get; set; }
        public IEnumerable<Guid> MentionedUserIds { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Enum ActivityType { get; set; }
    }
}