using System;
using Compent.CommandBus;
using Compent.Uintra.Core.Activity;
using Uintra.Comments;
using Uintra.Comments.CommandBus;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.Context.Extensions;

namespace Compent.Uintra.Core.CommandBus
{
    public class CommentHandle : IHandle<AddCommentCommand>, IHandle<EditCommentCommand>, IHandle<RemoveCommentCommand>
    {
        private readonly ICommentsService _commentsService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public CommentHandle(ICommentsService commentsService, IActivitiesServiceFactory activitiesServiceFactory)
        {
            _commentsService = commentsService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        public BroadcastResult Handle(AddCommentCommand command)
        {
            var commentsTarget = command.Context.GetCommentsTarget();

            _commentsService.Create(command.CreateDto);
            UpdateCache(commentsTarget.Type, commentsTarget.EntityId.Value);

            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(EditCommentCommand command)
        {
            var commentsTarget = command.Context.GetCommentsTarget();

            _commentsService.Update(command.EditDto);
            UpdateCache(commentsTarget.Type, commentsTarget.EntityId.Value);

            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(RemoveCommentCommand command)
        {
            var commentsTarget = command.Context.GetCommentsTarget();

            _commentsService.Delete(command.CommentId);
            UpdateCache(commentsTarget.Type, commentsTarget.EntityId.Value);

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
