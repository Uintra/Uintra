using System;

namespace Uintra.Features.Groups.CommandBus.Commands
{
    public class HideGroupCommand : GroupCommand
    {
        public HideGroupCommand(Guid groupId)
            : base(groupId)
        {
        }
    }
}