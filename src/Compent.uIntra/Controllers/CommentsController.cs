using System;
using System.Web.Mvc;
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

        private readonly ICustomCommentableService _customCommentableService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ICommentsService _commentsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotificationTypeProvider _notificationTypeProvider;
        private readonly IUmbracoContentHelper _umbracoContentHelper;

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
            _customCommentableService = customCommentableService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _commentsService = commentsService;
            _intranetUserService = intranetUserService;
            _notificationTypeProvider = notificationTypeProvider;
            _umbracoContentHelper = umbracoContentHelper;
        }

        protected override void OnCommentCreated(Comment comment)
        {
            var service = _activitiesServiceFactory.GetService<INotifyableService>(comment.ActivityId);
            if (service != null)
            {
                var notificationId = comment.ParentId.HasValue
                    ? NotificationTypeEnum.CommentReplied.ToInt()
                    : NotificationTypeEnum.CommentAdded.ToInt();

                var notificationType = _notificationTypeProvider.Get(notificationId);
                service.Notify(comment.ParentId ?? comment.Id, notificationType);
            }
        }

        protected override void OnCommentEdited(Comment comment)
        {
            var service = _activitiesServiceFactory.GetService<INotifyableService>(comment.ActivityId);
            if (service != null)
            {
                var notificationType = _notificationTypeProvider.Get(NotificationTypeEnum.CommentEdited.ToInt());
                service.Notify(comment.Id, notificationType);
            }
        }

        [HttpPost]
        public override PartialViewResult Add(CommentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return OverView(model.ActivityId);
            }

            if (_umbracoContentHelper.IsForContentPage(model.ActivityId))
            {
                _customCommentableService.CreateComment(_intranetUserService.GetCurrentUser().Id, model.ActivityId, model.Text, model.ParentId);
                return OverView(model.ActivityId);
            }


            var service = _activitiesServiceFactory.GetService<ICommentableService>(model.ActivityId);
            var comment = service.CreateComment(_intranetUserService.GetCurrentUser().Id, model.ActivityId, model.Text, model.ParentId);
            OnCommentCreated(comment);

            return OverView(model.ActivityId);
        }

        [HttpPut]
        public override PartialViewResult Edit(CommentEditModel model)
        {
            var comment = _commentsService.Get(model.Id);

            if (!ModelState.IsValid || !_commentsService.CanEdit(comment, _intranetUserService.GetCurrentUser().Id))
            {
                return OverView(model.Id);
            }

            if (_umbracoContentHelper.IsForContentPage(comment.ActivityId))
            {
                _customCommentableService.UpdateComment(model.Id, model.Text);
                return OverView(comment.ActivityId);
            }


            var service = _activitiesServiceFactory.GetService<ICommentableService>(comment.ActivityId);
            service.UpdateComment(model.Id, model.Text);
            OnCommentEdited(comment);
            return OverView(comment.ActivityId);
        }

        [HttpDelete]
        public override PartialViewResult Delete(Guid id)
        {
            var comment = _commentsService.Get(id);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;

            if (!_commentsService.CanDelete(comment, currentUserId))
            {
                return OverView(comment.ActivityId);
            }

            if (_umbracoContentHelper.IsForContentPage(comment.ActivityId))
            {
                _customCommentableService.DeleteComment(id);
                return OverView(comment.ActivityId);
            }

            var service = _activitiesServiceFactory.GetService<ICommentableService>(comment.ActivityId);
            service.DeleteComment(id);

            return OverView(comment.ActivityId);
        }
    }
}