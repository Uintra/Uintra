using System;
using System.Collections.Generic;
using Compent.CommandBus;

namespace Uintra20.Core.User.Commands
{
    public class MentionCommand : ICommand
    {
        public Guid MentionedSourceId { get; set; }
        public IEnumerable<Guid> MentionedUserIds { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Enum ActivityType { get; set; }
    }
}