using Compent.CommandBus;
using Uintra20.Core.Activity;
using Uintra20.Features.Likes.CommandBus.Commands;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Likes.CommandBus
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

            switch (command.EntityType.ToInt())
            {
                case (int)ContextType.Comment:
                    if (ContextExtensions.HasFlagScalar(command.EntityType, ContextType.Comment | ContextType.Activity | ContextType.PagePromotion | ContextType.ContentPage))
                    {
                        var service = _activitiesServiceFactory.GetNotifyableService(command.EntityId);
                        service.Notify(likeTargetEntityId, NotificationTypeEnum.CommentLikeAdded);
                    }
                    break;

                case int type when ContextExtensions.HasFlagScalar(type, ContextType.Activity):
                    var notifiableService = _activitiesServiceFactory.GetNotifyableService(likeTargetEntityId);
                    notifiableService.Notify(likeTargetEntityId, NotificationTypeEnum.ActivityLikeAdded);
                    break;
            }

            return BroadcastResult.Success;
        }
    }
}