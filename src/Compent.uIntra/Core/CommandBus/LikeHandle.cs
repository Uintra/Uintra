using System;
using Compent.CommandBus;
using Compent.Uintra.Core.Activity;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Likes;
using Uintra.Likes.CommandBus;

namespace Compent.Uintra.Core.CommandBus
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
            var likeTarget = command.Context.Value;
            var likeTargetEntityId = likeTarget.EntityId.Value;

            _likesService.Add(command.Author, likeTargetEntityId);
            UpdateCache(likeTarget.Type, likeTargetEntityId);

            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(RemoveLikeCommand command)
        {
            var likeTarget = command.Context.Value;
            var likeTargetEntityId = likeTarget.EntityId.Value;

            _likesService.Remove(command.Author, likeTargetEntityId);
            UpdateCache(likeTarget.Type, likeTargetEntityId);

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