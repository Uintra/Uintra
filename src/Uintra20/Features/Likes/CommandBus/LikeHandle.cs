using System;
using Compent.CommandBus;
using Uintra20.Core.Activity;
using Uintra20.Features.Likes.CommandBus.Commands;
using Uintra20.Features.Likes.Services;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Likes.CommandBus
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
            if (!ContextExtensions.HasFlagScalar(commentsTargetType, ContextType.Activity | ContextType.PagePromotion)) return;

            var activityService = _activitiesServiceFactory.GetCacheableIntranetActivityService(commentsTargetEntityId);
            activityService.UpdateActivityCache(commentsTargetEntityId);
        }
    }
}