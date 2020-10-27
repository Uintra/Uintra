using System;
using Compent.CommandBus;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Features.Likes.CommandBus.Commands;
using Uintra.Features.Likes.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Likes.CommandBus
{
    public class LikeHandle : IHandle<AddLikeCommand>, IHandle<RemoveLikeCommand>
    {
        private readonly ILikesService _likesService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public LikeHandle(ILikesService likesService, IActivitiesServiceFactory activitiesServiceFactory)
        {
            _likesService = likesService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        public BroadcastResult Handle(AddLikeCommand command)
        {
            var likeTargetEntityId = command.EntityId;

            _likesService.Add(command.Author, likeTargetEntityId);
            UpdateCache(command.EntityType, likeTargetEntityId);

            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(RemoveLikeCommand command)
        {
            var likeTargetEntityId = command.EntityId;

            _likesService.Remove(command.Author, likeTargetEntityId);
            UpdateCache(command.EntityType, likeTargetEntityId);

            return BroadcastResult.Success;
        }

        private void UpdateCache(Enum commentsTargetType, Guid commentsTargetEntityId)
        {
            if (!commentsTargetType.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events)) return;

            var activityService = _activitiesServiceFactory.GetCacheableIntranetActivityService(commentsTargetEntityId);
            activityService.UpdateActivityCache(commentsTargetEntityId);
        }
    }
}