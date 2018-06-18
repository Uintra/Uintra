using System;

namespace Uintra.Groups.CommandBus
{
    public class UnhideGroupCommand : GroupCommand
    {
        public UnhideGroupCommand(Guid groupId)
            : base(groupId)
        {
        }
    }
}