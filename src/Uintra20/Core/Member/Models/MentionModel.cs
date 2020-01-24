using System;
using System.Collections.Generic;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Member.Models
{
    public class MentionModel
    {
        public Guid MentionedSourceId { get; set; }
        public IEnumerable<Guid> MentionedUserIds { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public UintraLinkModel Url { get; set; }
        public Enum ActivityType { get; set; }
    }
}