using System;
using Compent.CommandBus;

namespace Uintra.Groups.CommandBus
{
    public abstract class GroupCommand : ICommand
    {
        public Guid GroupId { get; }

        protected GroupCommand(Guid groupId)
        {
            GroupId = groupId;
        }
    }
}