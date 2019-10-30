﻿using System;
using Uintra.Comments;
using Uintra.Core.Activity;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Uintra.Notification.Core.Entities;
using Umbraco.Core.Models;

namespace Compent.Uintra.Core.Helpers
{
    public interface INotifierDataHelper
    {
        ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        ActivityReminderDataModel GetActivityReminderDataModel(IIntranetActivity activity, Enum notificationType);
        CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, CommentModel comment, Enum notificationType, Guid notifierId);
        CommentNotifierDataModel GetCommentNotifierDataModel(IPublishedContent content, CommentModel comment, Enum notificationType, Guid notifierId);
        LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId);
        GroupInvitationDataModel GetGroupInvitationDataModel(NotificationTypeEnum notificationType, Guid groupId, Guid notifierId, Guid receiverId);
    }
}