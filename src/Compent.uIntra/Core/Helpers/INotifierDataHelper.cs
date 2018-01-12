using System;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;
using uIntra.Notification;

namespace Compent.uIntra.Core.Helpers
{
    public interface INotifierDataHelper
    {
        ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, IIntranetType notificationType, Guid notifierId);
        ActivityReminderDataModel GetActivityReminderDataModel(IIntranetActivity activity, IIntranetType notificationType);
        CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, Comment comment, IIntranetType notificationType, Guid notifierId);
        LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, IIntranetType notificationType, Guid notifierId);
    }
}