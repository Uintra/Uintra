using System;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Events;
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

        public CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, Comment comment, IIntranetType activityType, Guid notifierId)
        {
            return new CommentNotifierDataModel
            {
                CommentId = comment.Id,
                ActivityType = activityType,
                NotifierId = notifierId,
                Title = GetNotifierDataTitle(activity),
                Url = _commentLinkHelper.GetDetailsUrlWithComment(activity.Id, comment.Id)
            };
        }

        public ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, IIntranetType activityType, Guid notifierId)
        {
            return new ActivityNotifierDataModel
            {
                ActivityType = activityType,
                Title = GetNotifierDataTitle(activity),
                Url = _linkService.GetLinks(activity.Id).Details,
                NotifierId = notifierId
            };
        }

        public LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, IIntranetType activityType, Guid notifierId)
        {
            return new LikesNotifierDataModel
            {
                Title = GetNotifierDataTitle(activity),
                ActivityType = activityType,
                NotifierId = notifierId,
                CreatedDate = DateTime.Now,
                Url = _linkService.GetLinks(activity.Id).Details
            };
        }

        public ActivityReminderDataModel GetActivityReminderDataModel(EventBase @event, IIntranetType activityType)
        {
            return new ActivityReminderDataModel
            {
                Url = _linkService.GetLinks(@event.Id).Details,
                Title = @event.Title,
                ActivityType = activityType
            };
        }

        private static string GetNotifierDataTitle(IIntranetActivity activity)
        {
            return activity.Type.Id == IntranetActivityTypeEnum.Bulletins.ToInt() ? activity.Description : activity.Title;
        }
    }
}