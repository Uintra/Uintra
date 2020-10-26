using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.CommandBus;
using Uintra.Core.Member.Commands;
using Uintra.Features.Notification.Models;
using Uintra.Features.Notification.Services;

namespace Uintra.Core.Member.CommandBus
{
    public class MentionHandle : IHandle<MentionCommand>
    {
        private readonly IUserMentionNotificationService _notificationService;
        private readonly INotificationsService _notificationsService;

        public MentionHandle(IUserMentionNotificationService notificationService)
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
                MentionedSourceId = command.MentionedSourceId,
                ActivityType = command.ActivityType
            };

            _notificationService.SendNotification(notifierData);

            return BroadcastResult.Success;
        }
    }
}