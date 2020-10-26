using System;
using Compent.CommandBus;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Features.Comments.CommandBus.Commands;
using Uintra.Features.Comments.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Comments.CommandBus
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
            _commentsService.Create(command.CreateDto);
            UpdateCache(command.TargetType, command.TargetId);

            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(EditCommentCommand command)
        {
            _commentsService.Update(command.EditDto);
            UpdateCache(command.TargetType, command.TargetId);

            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(RemoveCommentCommand command)
        {
            _commentsService.Delete(command.CommentId);
            UpdateCache(command.TargetType, command.TargetId);

            return BroadcastResult.Success;
        }

        private void UpdateCache(Enum commentsTargetType, Guid commentsTargetEntityId)
        {
            if (commentsTargetType.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events))
            {
                var activityService = _activitiesServiceFactory.GetCacheableIntranetActivityService(commentsTargetEntityId);
                activityService.UpdateActivityCache(commentsTargetEntityId);
            }
        }
    }
}