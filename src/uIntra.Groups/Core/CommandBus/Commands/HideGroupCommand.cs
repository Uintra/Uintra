using System;

namespace Uintra.Groups.CommandBus
{
    public class HideGroupCommand : GroupCommand
    {
        public HideGroupCommand(Guid groupId)
            : base(groupId)
        {
        }
    }
}