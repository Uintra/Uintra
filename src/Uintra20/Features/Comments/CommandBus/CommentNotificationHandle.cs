using Compent.CommandBus;
using Uintra20.Core.Activity;
using Uintra20.Features.Comments.CommandBus.Commands;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Comments.CommandBus
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
            var commentsTargetEntityId = command.TargetId;

            var isHasFlag = ContextExtensions
                .HasFlagScalar(
                    command.TargetType,
                    ContextType.Activity | ContextType.PagePromotion | ContextType.ContentPage);

            if (!isHasFlag) 
                return BroadcastResult.Success;

            var notifiableService = _activitiesServiceFactory.GetNotifyableService(commentsTargetEntityId);

            var notificationType = command.CreateDto.ParentId.HasValue ?
                NotificationTypeEnum.CommentReplied :
                NotificationTypeEnum.CommentAdded;

            notifiableService.Notify(command.CreateDto.ParentId ?? command.CreateDto.Id, notificationType);

            return BroadcastResult.Success;
        }

        public BroadcastResult Handle(EditCommentCommand command)
        {
            var commentsTargetEntityId = command.TargetId;

            if (!ContextExtensions.HasFlagScalar(command.TargetType, ContextType.Activity))
                return BroadcastResult.Success;

            var notifiableService = _activitiesServiceFactory.GetNotifyableService(commentsTargetEntityId);
            notifiableService.Notify(command.EditDto.Id, NotificationTypeEnum.CommentEdited);

            return BroadcastResult.Success;
        }
    }
}