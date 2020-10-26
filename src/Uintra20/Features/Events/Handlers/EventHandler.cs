using Compent.CommandBus;
using System;
using Uintra20.Core.Activity;
using Uintra20.Features.Events.Entities;
using Uintra20.Features.Media.Intranet.Services.Contracts;
using Uintra20.Features.Media.Video.Commands;

namespace Uintra20.Features.Events.Handlers
{
    public class EventHandler : IHandle<VideoConvertedCommand>
    {

        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IIntranetActivityService<Entities.Event> _intranetActivityService;

        public EventHandler(
            IIntranetMediaService intranetMediaService,
            IIntranetActivityService<Event> intranetActivityService)
        {
            _intranetMediaService = intranetMediaService;
            _intranetActivityService = intranetActivityService;
        }

        public BroadcastResult Handle(VideoConvertedCommand command)
        {
            var entityId = _intranetMediaService.GetEntityIdByMediaId(command.MediaId);
            var entity = _intranetActivityService.Get(entityId);
            if (entity == null)
            {
                return BroadcastResult.Success;
            }

            entity.ModifyDate = DateTime.UtcNow;
            return BroadcastResult.Success;
        }
    }
}