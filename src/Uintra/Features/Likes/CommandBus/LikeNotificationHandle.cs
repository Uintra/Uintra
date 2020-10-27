using Compent.CommandBus;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Features.Likes.CommandBus.Commands;
using Uintra.Features.Notification.Configuration;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Likes.CommandBus
{
    public class LikeNotificationHandle : IHandle<AddLikeCommand>
    {
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public LikeNotificationHandle(IActivitiesServiceFactory activitiesServiceFactory)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        public BroadcastResult Handle(AddLikeCommand command)
        {
            var likeTargetEntityId = command.EntityId;

            switch (command.EntityType)
            {
                case IntranetEntityTypeEnum.Comment:
                    var service = _activitiesServiceFactory.GetNotifyableService(command.EntityId);
                    service.Notify(likeTargetEntityId, NotificationTypeEnum.CommentLikeAdded);
                    break;

                case IntranetEntityTypeEnum type when type.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events):
                    var notifiableService = _activitiesServiceFactory.GetNotifyableService(likeTargetEntityId);
                    notifiableService.Notify(likeTargetEntityId, NotificationTypeEnum.ActivityLikeAdded);
                    break;
            }

            return BroadcastResult.Success;
        }
    }
}