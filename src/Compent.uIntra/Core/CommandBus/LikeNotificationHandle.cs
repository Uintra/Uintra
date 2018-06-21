using Compent.CommandBus;
using Compent.Uintra.Core.Activity;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.Context.Extensions;
using Uintra.Core.Extensions;
using Uintra.Likes.CommandBus;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.CommandBus
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
            var likeTarget = command.Context.Value;
            var likeTargetEntityId = likeTarget.EntityId.Value;

            switch (likeTarget.Type.ToInt())
            {
                case (int)ContextType.Comment:
                    var commentsTarget = command.Context.GetCommentsTarget();

                    if (ContextExtensions.HasFlagScalar(commentsTarget.Type, ContextType.Activity))
                    {
                        var service = _activitiesServiceFactory.GetNotifyableService(commentsTarget.EntityId.Value);
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
