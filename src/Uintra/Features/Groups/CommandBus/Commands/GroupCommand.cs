using System;
using Compent.CommandBus;

namespace Uintra.Features.Groups.CommandBus.Commands
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