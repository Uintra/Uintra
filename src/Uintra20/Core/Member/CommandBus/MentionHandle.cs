using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.CommandBus;
using Uintra20.Core.Member.Commands;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Core.Member.CommandBus
{
    public class MentionHandle : IHandle<MentionCommand>
    {
        private readonly IUserMentionNotificationService _notificationService;

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