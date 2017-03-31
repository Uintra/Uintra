using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.Activity;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.Comments
{
    public class CommentsController : SurfaceController
    {
        private readonly ICommentsService _commentsService;
        private readonly IIntranetUserService _intranetUserService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public CommentsController(
            ICommentsService commentsService,
            IIntranetUserService intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _commentsService = commentsService;
            _intranetUserService = intranetUserService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        [HttpPost]
        public PartialViewResult Add(CommentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return OverView(model.ActivityId);
            }

            var service = _activitiesServiceFactory.GetService(model.ActivityId);
            var commentableService = (ICommentableService)service;
            commentableService.CreateComment(_intranetUserService.GetCurrentUser().Id, model.ActivityId, model.Text, model.ParentId);

            return OverView(model.ActivityId);
        }

        [HttpPut]
        public PartialViewResult Edit(CommentEditModel model)
        {
            var comment = _commentsService.Get(model.Id);


            if (!ModelState.IsValid || !_commentsService.CanEdit(comment, _intranetUserService.GetCurrentUser().Id))
            {
                return OverView(comment.Id);
            }

            var service = _activitiesServiceFactory.GetService(comment.ActivityId);
            var commentableService = (ICommentableService)service;
            commentableService.UpdateComment(model.Id, model.Text);
            return OverView(comment.ActivityId);
        }

        [HttpDelete]
        public PartialViewResult Delete(Guid id)
        {
            var comment = _commentsService.Get(id);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;

            if (!_commentsService.CanDelete(comment, currentUserId))
            {
                return OverView(comment.ActivityId);
            }

            var service = _activitiesServiceFactory.GetService(comment.ActivityId);
            var commentableService = (ICommentableService)service;
            commentableService.DeleteComment(id);

            return OverView(comment.ActivityId);
        }

        public PartialViewResult CreateView(Guid activityId)
        {
            var model = new CommentCreateModel
            {
                ActivityId = activityId,
                UpdateElementId = GetOverviewElementId(activityId)
            };
            return PartialView("~/App_Plugins/Comments/View/CommentsCreateView.cshtml", model);
        }

        public PartialViewResult EditView(Guid id, string updateElementId)
        {
            var comment = _commentsService.Get(id);
            var model = new CommentEditModel
            {
                Id = id,
                Text = comment.Text,
                UpdateElementId = updateElementId
            };
            return PartialView("~/App_Plugins/Comments/View/CommentsEditView.cshtml", model);
        }

        public PartialViewResult OverView(ICommentable commentsInfo)
        {
            return OverView(commentsInfo.Id, commentsInfo.Comments);
        }

        public PartialViewResult PreView(ICommentable commentsInfo, string link)
        {
            var model = new CommentPreviewModel
            {
                Count = commentsInfo.Comments.Count(),
                Link = $"{link}#{GetOverviewElementId(commentsInfo.Id)}"
            };
            return PartialView("~/App_Plugins/Comments/View/CommentsPreView.cshtml", model);
        }

        private PartialViewResult OverView(Guid activityId)
        {
            return OverView(activityId, _commentsService.GetMany(activityId));
        }

        private PartialViewResult OverView(Guid activityId, IEnumerable<Comment> comments)
        {
            var model = new CommentsOverviewModel
            {
                ActivityId = activityId,
                Comments = GetCommentViews(comments),
                ElementId = GetOverviewElementId(activityId)
            };

            return PartialView("~/App_Plugins/Comments/View/CommentsOverView.cshtml", model);
        }

        private IEnumerable<CommentViewModel> GetCommentViews(IEnumerable<Comment> comments)
        {
            comments = comments.OrderBy(c => c.CreatedDate);
            var commentsList = comments as List<Comment> ?? comments.ToList();
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            var creators = _intranetUserService.GetAll().ToList();
            var replies = commentsList.FindAll(_commentsService.IsReply);

            foreach (var comment in commentsList.FindAll(c => !_commentsService.IsReply(c)))
            {
                var model = GetCommentView(comment, currentUserId, creators.SingleOrDefault(c => c.Id == comment.UserId));
                var commentReplies = replies.FindAll(reply => reply.ParentId == model.Id);
                model.Replies = commentReplies.Select(reply => GetCommentView(reply, currentUserId, creators.SingleOrDefault(c => c.Id == reply.UserId)));
                yield return model;
            }
        }

        private CommentViewModel GetCommentView(Comment comment, Guid currentUserId, IIntranetUser creator)
        {
            var model = comment.Map<CommentViewModel>();
            model.ModifyDate = _commentsService.WasChanged(comment) ? comment.ModifyDate : default(DateTime?);
            model.CanEdit = _commentsService.CanEdit(comment, currentUserId);
            model.CanDelete = _commentsService.CanDelete(comment, currentUserId);
            model.CreatorFullName = creator?.DisplayedName;
            model.Photo = creator?.Photo;
            model.ElementOverviewId = GetOverviewElementId(comment.ActivityId);
            model.CommentViewId = _commentsService.GetCommentViewId(comment.Id);
            return model;
        }

        private static string GetOverviewElementId(Guid activityId)
        {
            return $"js-comments-overview-{activityId}";
        }
    }
}