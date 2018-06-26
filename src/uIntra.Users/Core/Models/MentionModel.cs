using System;
using System.Collections.Generic;

namespace Uintra.Users
{
    public class MentionModel
    {
        public IEnumerable<Guid> MentionedUserIds { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}