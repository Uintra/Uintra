using Compent.CommandBus;
using Compent.Uintra.Core.Notification;
using Uintra.Users.Commands;

namespace Compent.Uintra.Core.CommandBus
{
    public class MentionHandle : IHandle<MentionCommand>
    {
        private readonly IUserMentionNotificationService _notificationService;

        public MentionHandle
        (
            IUserMentionNotificationService notificationService
        )
        {
            _notificationService = notificationService;
        }
        public BroadcastResult Handle(MentionCommand command)
        {

            var notifierData = new UserMentionNotificationModel()
            {
                CreatorId = command.CreatorId,
                ReceivedIds = command.MentionedUserIds,
                Title = command.Title,
                Url = command.Url,
                MentionedSourceId = command.MentionedSourceId
            };

            _notificationService.SendNotification(notifierData);

            return BroadcastResult.Success;
        }
    }
}