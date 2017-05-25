using System;
using System.Web.Mvc;
using Compent.uIntra.Core.Comments;
using uIntra.Comments;
using uIntra.Comments.Web;
using uIntra.Core.Activity;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Configuration;
using uIntra.Users;

namespace Compent.uIntra.Controllers
{
    public class CommentsController : CommentsControllerBase
    {
        protected override string OverviewViewPath { get; } = "~/Views/Comments/CommentsOverView.cshtml";

        private readonly ICommentableService _customCommentableService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly ICommentsService _commentsService;
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;

        public CommentsController(
            ICommentsService commentsService,
            IIntranetUserService<IntranetUser> intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory,
            ICommentableService customCommentableService)
            : base(commentsService, intranetUserService, activitiesServiceFactory)
        {
            _customCommentableService = customCommentableService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _commentsService = commentsService;
            _intranetUserService = intranetUserService;
        }

        protected override void OnCommentCreated(Comment comment)
        {
            var service = _activitiesServiceFactory.GetServiceSafe<INotifyableService>(comment.ActivityId);
            if (service != null)
            {
                service.Notify(comment.ParentId ?? comment.Id,
                    comment.ParentId.HasValue
                        ? NotificationTypeEnum.CommentReplyed
                        : NotificationTypeEnum.CommentAdded);
            }
        }

        protected override void OnCommentEdited(Comment comment)
        {
            var service = _activitiesServiceFactory.GetService<INotifyableService>(comment.ActivityId);
            if (service != null)
            {
                service.Notify(comment.Id, NotificationTypeEnum.CommentEdited);
            }
        }

        [HttpPost]
        public override PartialViewResult Add(CommentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return OverView(model.ActivityId);
            }

            if (model.ActivityId == CommentsTestConstants.ActivityId)
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

            if (comment.ActivityId == CommentsTestConstants.ActivityId)
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

            if (comment.ActivityId == CommentsTestConstants.ActivityId)
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