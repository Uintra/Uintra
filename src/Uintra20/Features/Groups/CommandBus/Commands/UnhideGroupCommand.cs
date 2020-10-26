using System;

namespace Uintra20.Features.Groups.CommandBus.Commands
{
    public class UnhideGroupCommand : GroupCommand
    {
        public UnhideGroupCommand(Guid groupId)
            : base(groupId)
        {
        }
    }
}