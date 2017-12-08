using System;
using Compent.uIntra.Core.Helpers;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;

namespace Compent.uIntra.Installer.Migrations
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

        internal INotifierDataValue GetNotifierDataValue(OldNotifierData oldData, IIntranetActivity activity, IIntranetType notificationType)
        {
            INotifierDataValue result;

            switch (notificationType.Id)
            {
                case (int)NotificationTypeEnum.CommentAdded:
                case (int)NotificationTypeEnum.CommentReplied:
                {
                    var commentId = ParseCommentId(oldData.Url);
                    var comment = _commentsService.Get(commentId);
                    result = _notifierDataHelper.GetCommentNotifierDataModel(activity, comment, notificationType, oldData.NotifierId);
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