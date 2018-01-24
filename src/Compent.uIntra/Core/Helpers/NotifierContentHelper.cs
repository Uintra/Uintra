using System;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Notification;

namespace Compent.uIntra.Core.Helpers
{
    public class NotifierDataHelper : INotifierDataHelper
    {
        private readonly IActivityLinkService _linkService;
        private readonly ICommentLinkHelper _commentLinkHelper;

        public NotifierDataHelper(IActivityLinkService linkService, ICommentLinkHelper commentLinkHelper)
        {
            _linkService = linkService;
            _commentLinkHelper = commentLinkHelper;
        }

        public CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, CommentModel comment, Enum notificationType, Guid notifierId)
        {
            return new CommentNotifierDataModel
            {
                CommentId = comment.Id,
                NotificationType = notificationType,
                NotifierId = notifierId,
                Title = GetNotifierDataTitle(activity),
                Url = _commentLinkHelper.GetDetailsUrlWithComment(activity.Id, comment.Id),
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId)
        {
            return new ActivityNotifierDataModel
            {
                NotificationType = notificationType,
                ActivityType = activity.Type,
                Title = GetNotifierDataTitle(activity),
                Url = _linkService.GetLinks(activity.Id).Details,
                NotifierId = notifierId,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, Enum notificationType, Guid notifierId)
        {
            return new LikesNotifierDataModel
            {
                Title = GetNotifierDataTitle(activity),
                NotificationType = notificationType,
                ActivityType = activity.Type,
                NotifierId = notifierId,
                CreatedDate = DateTime.Now,
                Url = _linkService.GetLinks(activity.Id).Details,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        public ActivityReminderDataModel GetActivityReminderDataModel(IIntranetActivity activity, Enum activityType)
        {
            return new ActivityReminderDataModel
            {
                Url = _linkService.GetLinks(activity.Id).Details,
                Title = activity.Title,
                NotificationType = activityType,
                ActivityType = activity.Type,
                IsPinned = activity.IsPinned,
                IsPinActual = activity.IsPinActual
            };
        }

        private static string GetNotifierDataTitle(IIntranetActivity activity) 
            => activity.Type.Id == IntranetActivityTypeEnum.Bulletins.ToInt() ? activity.Description : activity.Title;
    }
}