using System;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;
using uIntra.Events;
using uIntra.Notification;

namespace Compent.uIntra.Core.Helpers
{
    public interface INotifierDataHelper
    {
        ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, IIntranetType activityType, Guid notifierId);
        ActivityReminderDataModel GetActivityReminderDataModel(EventBase @event, IIntranetType activityType);
        CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, Comment comment, IIntranetType activityType, Guid notifierId);
        LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, IIntranetType activityType, Guid notifierId);
    }
}