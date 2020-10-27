using System;
using System.Collections.Generic;
using System.Linq;
using Compent.CommandBus;
using Uintra.Core.Activity;
using Uintra.Core.Activity.Entities;
using Uintra.Features.Groups.CommandBus.Commands;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Groups.CommandBus
{
    public class GroupActivitiesHandle : IHandle<HideGroupCommand>, IHandle<UnhideGroupCommand>
    {
        private readonly IEnumerable<IIntranetActivityService<IIntranetActivity>> _intranetActivityServices;

        public GroupActivitiesHandle(IEnumerable<IIntranetActivityService<IIntranetActivity>> intranetActivityServices)
        {
            _intranetActivityServices = intranetActivityServices;
        }

        public BroadcastResult Handle(HideGroupCommand command)
        {
            UpdateGroupActivityCache(command.GroupId);
            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(UnhideGroupCommand command)
        {
            UpdateGroupActivityCache(command.GroupId);
            return BroadcastResult.Success;
        }

        private void UpdateGroupActivityCache(Guid groupId)
        {
            foreach (var activityService in _intranetActivityServices)
            {
                var groupActivities = activityService
                    .GetAll()
                    .TryCast<IGroupActivity>()
                    .Where(activity => activity.GroupId == groupId)
                    .ToList();

                if (groupActivities.Count > 0)
                {
                    var cacheableIntranetActivityService = (ICacheableIntranetActivityService<IIntranetActivity>)activityService;
                    groupActivities.ForEach(activity => cacheableIntranetActivityService.UpdateActivityCache(activity.Id));
                }
            }
        }
    }
}