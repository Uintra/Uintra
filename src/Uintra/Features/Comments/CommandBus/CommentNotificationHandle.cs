using Compent.CommandBus;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Features.Comments.CommandBus.Commands;
using Uintra.Features.Notification.Configuration;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Comments.CommandBus
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

            //var isHasFlag = command.TargetType.Is(IntranetEntityTypeEnum.ContentPage, IntranetEntityTypeEnum.News,
            //                                                IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events);

            //if (!isHasFlag) 
            //    return BroadcastResult.Success;

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

            if (!command.TargetType.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social,
                IntranetEntityTypeEnum.Events)) return BroadcastResult.Success;
            
            var notifiableService = _activitiesServiceFactory.GetNotifyableService(commentsTargetEntityId);
            notifiableService.Notify(command.EditDto.Id, NotificationTypeEnum.CommentEdited);

            return BroadcastResult.Success;

        }
    }
}