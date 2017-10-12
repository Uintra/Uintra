using System;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Events;
using uIntra.Notification;

namespace Compent.uIntra.Core.Helpers
{
    public class NotifierDataHelper : INotifierDataHelper
    {
        private readonly IActivityLinkService _linkService;
        private readonly ICommentLinkHelper _commentLinkHelper;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public NotifierDataHelper(IActivityLinkService linkService, ICommentLinkHelper commentLinkHelper, IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _linkService = linkService;
            _commentLinkHelper = commentLinkHelper;
            _intranetUserService = intranetUserService;
        }

        public CommentNotifierDataModel GetCommentNotifierDataModel(IIntranetActivity activity, Comment comment, IIntranetType activityType)
        {
            return new CommentNotifierDataModel
            {
                CommentId = comment.Id,
                ActivityType = activityType,
                NotifierId = _intranetUserService.GetCurrentUser().Id,
                Title = activity.Title,
                Url = _commentLinkHelper.GetDetailsUrlWithComment(activity.Id, comment.Id)
            };
        }

        public ActivityNotifierDataModel GetActivityNotifierDataModel(IIntranetActivity activity, IIntranetType activityType)
        {
            return new ActivityNotifierDataModel
            {
                ActivityType = activityType,
                Title = activity.Title,
                Url = _linkService.GetLinks(activity.Id).Details,
                NotifierId = activity.Id
            };
        }

        public LikesNotifierDataModel GetLikesNotifierDataModel(IIntranetActivity activity, IIntranetType activityType) 
        {
            return new LikesNotifierDataModel
            {
                Title = activity.Description,
                ActivityType = activityType,
                NotifierId = _intranetUserService.GetCurrentUser().Id,
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
                ActivityType = activityType,
                StartDate = @event.StartDate
            };
        }


    }
}