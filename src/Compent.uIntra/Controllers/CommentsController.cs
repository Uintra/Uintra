using Compent.uIntra.Core.Comments;
using Localization.Umbraco.Attributes;
using uIntra.Comments;
using uIntra.Comments.Web;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
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
                var notificationId = comment.ParentId.HasValue ?
                    NotificationTypeEnum.CommentReplied.ToInt() :
                    NotificationTypeEnum.CommentAdded.ToInt();

                var notificationType = _notificationTypeProvider.Get(notificationId);
                service.Notify(comment.ParentId ?? comment.Id, notificationType);
            }
        }

        protected override void OnCommentEdited(CommentModel comment)
        {
            if (IsForPagePromotion(comment.ActivityId)) return;

            var service = _activitiesServiceFactory.GetService<INotifyableService>(comment.ActivityId);
            if (service != null)
            {
                var notificationType = _notificationTypeProvider.Get(NotificationTypeEnum.CommentEdited.ToInt());
                service.Notify(comment.Id, notificationType);
            }
        }
    }
}