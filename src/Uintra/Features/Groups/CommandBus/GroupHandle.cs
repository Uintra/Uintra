﻿using Compent.CommandBus;
using Uintra.Features.Groups.CommandBus.Commands;
using Uintra.Features.Groups.Services;

namespace Uintra.Features.Groups.CommandBus
{
    public class GroupHandle : IHandle<HideGroupCommand>, IHandle<UnhideGroupCommand>
    {
        private readonly IGroupService _groupService;

        public GroupHandle(IGroupService groupService)
        {
            _groupService = groupService;
        }

        public BroadcastResult Handle(HideGroupCommand command)
        {
            _groupService.Hide(command.GroupId);
            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(UnhideGroupCommand command)
        {
            _groupService.Unhide(command.GroupId);
            return BroadcastResult.Success;
        }
    }
}