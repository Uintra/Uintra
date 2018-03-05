using System;
using Compent.Uintra.Core.Helpers;
using Uintra.Comments;
using Uintra.Core.Activity;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Updater.Migrations._0._0._0._1.OldNotifications
{
    public class NewNotifierDataValueProvider
    {
        private readonly ICommentsService _commentsService;
        private readonly INotifierDataHelper _notifierDataHelper;

        public NewNotifierDataValueProvider(ICommentsService commentsService, INotifierDataHelper notifierDataHelper)
        {
            _commentsService = commentsService;
            _notifierDataHelper = notifierDataHelper;
        }

        internal INotifierDataValue GetNotifierDataValue(OldNotifierData oldData, IIntranetActivity activity, Enum notificationType)
        {
            INotifierDataValue result;

            switch (notificationType)
            {
                case NotificationTypeEnum.ActivityLikeAdded:
                    {
                        result = _notifierDataHelper.GetLikesNotifierDataModel(activity, notificationType, oldData.NotifierId);
                        break;
                    }
                case NotificationTypeEnum.CommentAdded:
                case NotificationTypeEnum.CommentReplied:
                case NotificationTypeEnum.CommentEdited:
                case NotificationTypeEnum.CommentLikeAdded:
                    {
                        var commentId = ParseCommentId(oldData.Url);
                        var comment = _commentsService.Get(commentId);
                        result = _notifierDataHelper.GetCommentNotifierDataModel(activity, comment, notificationType, oldData.NotifierId);
                        break;
                    }

                case NotificationTypeEnum.BeforeStart:
                    {
                        result = _notifierDataHelper.GetActivityReminderDataModel(activity, notificationType);
                        break;
                    }

                case NotificationTypeEnum.EventHided:
                case NotificationTypeEnum.EventUpdated:
                    {

                        result = _notifierDataHelper.GetActivityNotifierDataModel(activity, notificationType, oldData.NotifierId);
                        break;
                    }

                default:
                    result = null;
                    break;
            }

            return result;
        }

        private Guid ParseCommentId(string url) => url.ParseIdFromQueryString("#js-comment-view-");
    }
}