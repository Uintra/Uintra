using Compent.CommandBus;
using Compent.Uintra.Core.Activity;
using Uintra.Comments.CommandBus;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.Context.Extensions;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.CommandBus
{
    public class CommentNotificationHandle : IHandle<AddCommentCommand>, IHandle<EditCommentCommand>
    {
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public CommentNotificationHandle(IActivitiesServiceFactory activitiesServiceFactory)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        public BroadcastResult Handle(AddCommentCommand command)
        {
            var commentsTarget = command.Context.GetCommentsTarget();
            var commentsTargetEntityId = commentsTarget.EntityId.Value;

            if (ContextExtensions.HasFlagScalar(commentsTarget.Type, ContextType.Activity))
            {
                var notifiableService = _activitiesServiceFactory.GetNotifyableService(commentsTargetEntityId);

                var notificationType = command.CreateDto.ParentId.HasValue ?
                                           NotificationTypeEnum.CommentReplied :
                                           NotificationTypeEnum.CommentAdded;

                notifiableService.Notify(command.CreateDto.ParentId ?? command.CreateDto.Id, notificationType);
            }

            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(EditCommentCommand command)
        {
            var commentsTarget = command.Context.GetCommentsTarget();
            var commentsTargetEntityId = commentsTarget.EntityId.Value;

            if (ContextExtensions.HasFlagScalar(commentsTarget.Type, ContextType.Activity))
            {
                var notifiableService = _activitiesServiceFactory.GetNotifyableService(commentsTargetEntityId);
                notifiableService.Notify(command.EditDto.Id, NotificationTypeEnum.CommentEdited);
            }

            return BroadcastResult.Success;
        }
    }
}
