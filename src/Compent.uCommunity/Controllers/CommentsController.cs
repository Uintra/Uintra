using uCommunity.Comments;
using uCommunity.Comments.Core.Events;
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
            AfterCommentCreated += OnAfterCommentCreated;
            AfterCommentEdited += OnAfterCommentEdited;
        }

        private void OnAfterCommentEdited(object sender, CommentEdited model)
        {
            var service = ActivitiesServiceFactory.GetService(model.ActivityId);
            if (service is INotifyableService)
            {
                var notifyableService = (INotifyableService)service;
                notifyableService.Notify(model.Id, NotificationTypeEnum.CommentEdited);
            }
        }

        private void OnAfterCommentCreated(object sender, CommentCreated model)
        {
            var service = ActivitiesServiceFactory.GetService(model.ActivityId);
            if (service is INotifyableService)
            {
                var notifyableService = (INotifyableService) service;
                notifyableService.Notify(model.ParentId ?? model.Id,
                    model.ParentId.HasValue
                        ? NotificationTypeEnum.CommentReplyed
                        : NotificationTypeEnum.CommentAdded);
            }
        }
    }
}