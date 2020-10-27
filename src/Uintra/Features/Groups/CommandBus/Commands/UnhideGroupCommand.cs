using System;

namespace Uintra.Features.Groups.CommandBus.Commands
{
    public class UnhideGroupCommand : GroupCommand
    {
        public UnhideGroupCommand(Guid groupId)
            : base(groupId)
        {
        }
    }
}