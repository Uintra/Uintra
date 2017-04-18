using uCommunity.Comments;
using uCommunity.Comments.Web;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Services;

namespace Compent.uCommunity.Controllers
{
    public class CommentsController: CommentsControllerBase
    {
        public CommentsController(ICommentsService commentsService, IIntranetUserService intranetUserService, IActivitiesServiceFactory activitiesServiceFactory) 
            : base(commentsService, intranetUserService, activitiesServiceFactory)
        {
        }

        protected override void OnCommentCreated(Comment comment)
        {
            var service = ActivitiesServiceFactory.GetService(comment.ActivityId);
            if (service is INotifyableService)
            {
                var notifyableService = (INotifyableService)service;
                notifyableService.Notify(comment.ParentId ?? comment.Id,
                    comment.ParentId.HasValue
                        ? NotificationTypeEnum.CommentReplyed
                        : NotificationTypeEnum.CommentAdded);
            }
        }

        protected override void OnCommentEdited(Comment comment)
        {
            var service = ActivitiesServiceFactory.GetService(comment.ActivityId);
            if (service is INotifyableService)
            {
                var notifyableService = (INotifyableService)service;
                notifyableService.Notify(comment.Id, NotificationTypeEnum.CommentEdited);
            }
        }
    }
}