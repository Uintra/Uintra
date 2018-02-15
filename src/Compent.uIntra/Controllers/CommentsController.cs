using Compent.Uintra.Core.Comments;
using Localization.Umbraco.Attributes;
using Uintra.Comments;
using Uintra.Comments.Web;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.Media;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
{
    [ThreadCulture]
    public class CommentsController : CommentsControllerBase
    {
        protected override string OverviewViewPath { get; } = "~/Views/Comments/CommentsOverView.cshtml";
        protected override string ViewPath { get; } = "~/Views/Comments/CommentsView.cshtml";

        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly INotificationTypeProvider _notificationTypeProvider;

        public CommentsController(
            ICommentsService commentsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IMediaHelper mediaHelper,
            ICustomCommentableService customCommentableService,
            INotificationTypeProvider notificationTypeProvider,
            IUmbracoContentHelper umbracoContentHelper,
            IProfileLinkProvider profileLinkProvider,
            UmbracoHelper umbracoHelper)
            : base(commentsService, intranetUserService, activitiesServiceFactory, customCommentableService, umbracoContentHelper, profileLinkProvider, umbracoHelper)
        {
            _activitiesServiceFactory = activitiesServiceFactory;
            _notificationTypeProvider = notificationTypeProvider;
        }

        protected override void OnCommentCreated(CommentModel comment)
        {
            if (IsForPagePromotion(comment.ActivityId)) return;

            var service = _activitiesServiceFactory.GetService<INotifyableService>(comment.ActivityId);
            if (service != null)
            {
                var notificationType = comment.ParentId.HasValue ?
                    NotificationTypeEnum.CommentReplied :
                    NotificationTypeEnum.CommentAdded;

                service.Notify(comment.ParentId ?? comment.Id, notificationType);
            }
        }

        protected override void OnCommentEdited(CommentModel comment)
        {
            if (IsForPagePromotion(comment.ActivityId)) return;

            var service = _activitiesServiceFactory.GetService<INotifyableService>(comment.ActivityId);
            if (service != null)
            {
                var notificationType = NotificationTypeEnum.CommentEdited;
                service.Notify(comment.Id, notificationType);
            }
        }
    }
}