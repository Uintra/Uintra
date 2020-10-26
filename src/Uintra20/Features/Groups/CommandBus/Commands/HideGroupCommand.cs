using System;

namespace Uintra20.Features.Groups.CommandBus.Commands
{
    public class HideGroupCommand : GroupCommand
    {
        public HideGroupCommand(Guid groupId)
            : base(groupId)
        {
        }
    }
}