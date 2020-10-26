﻿using System;
using Compent.CommandBus;
using Uintra.Core.Activity;
using Uintra.Features.Media.Intranet.Services.Contracts;
using Uintra.Features.Media.Video.Commands;

namespace Uintra.Features.News.Handlers
{
    public class NewsHandler : IHandle<VideoConvertedCommand>
    {
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IIntranetActivityService<Entities.News> _intranetActivityService;
        public NewsHandler(
            IIntranetMediaService intranetMediaService, 
            IIntranetActivityService<Entities.News> intranetActivityService)
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