using System;
using System.Collections.Generic;
using Compent.CommandBus;

namespace Uintra.Users.Commands
{
    public class MentionCommand : ICommand
    {
        public IEnumerable<Guid> MentionedUserIds { get; set; }
        public Guid CreatorId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
