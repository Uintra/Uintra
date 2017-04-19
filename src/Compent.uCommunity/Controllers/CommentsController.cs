using System;
using System.Web.Mvc;
using Compent.uCommunity.Core.Comments;
using uCommunity.Comments;
using uCommunity.Comments.Web;
using uCommunity.Core.Activity;
using uCommunity.Core.User;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Services;

namespace Compent.uCommunity.Controllers
{
    public class CommentsController : CommentsControllerBase
    {
        private readonly ICommentableService _customCommentableService;

        public CommentsController(
            ICommentsService commentsService,
            IIntranetUserService intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory,
            ICommentableService customCommentableService)
            : base(commentsService, intranetUserService, activitiesServiceFactory)
        {
            _customCommentableService = customCommentableService;
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

        [HttpPost]
        public override PartialViewResult Add(CommentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return OverView(model.ActivityId);
            }

            if (model.ActivityId == CommentsTestConstants.ActivityId)
            {
                _customCommentableService.CreateComment(IntranetUserService.GetCurrentUser().Id, model.ActivityId, model.Text, model.ParentId);
                return OverView(model.ActivityId);
            }

            var service = ActivitiesServiceFactory.GetService(model.ActivityId);
            var commentableService = (ICommentableService)service;
            var comment = commentableService.CreateComment(IntranetUserService.GetCurrentUser().Id, model.ActivityId, model.Text, model.ParentId);
            OnCommentCreated(comment);

            return OverView(model.ActivityId);
        }

        [HttpPut]
        public override PartialViewResult Edit(CommentEditModel model)
        {
            var comment = CommentsService.Get(model.Id);

            if (!ModelState.IsValid || !CommentsService.CanEdit(comment, IntranetUserService.GetCurrentUser().Id))
            {
                return OverView(model.Id);
            }

            if (comment.ActivityId == CommentsTestConstants.ActivityId)
            {
                _customCommentableService.UpdateComment(model.Id, model.Text);
                return OverView(comment.ActivityId);
            }

            var service = ActivitiesServiceFactory.GetService(comment.ActivityId);
            var commentableService = (ICommentableService)service;
            commentableService.UpdateComment(model.Id, model.Text);
            OnCommentEdited(comment);
            return OverView(comment.ActivityId);
        }

        [HttpDelete]
        public override PartialViewResult Delete(Guid id)
        {
            var comment = CommentsService.Get(id);
            var currentUserId = IntranetUserService.GetCurrentUser().Id;

            if (!CommentsService.CanDelete(comment, currentUserId))
            {
                return OverView(comment.ActivityId);
            }

            if (comment.ActivityId == CommentsTestConstants.ActivityId)
            {
                _customCommentableService.DeleteComment(id);
                return OverView(comment.ActivityId);
            }

            var service = ActivitiesServiceFactory.GetService(comment.ActivityId);
            var commentableService = (ICommentableService)service;
            commentableService.DeleteComment(id);

            return OverView(comment.ActivityId);
        }
    }
}