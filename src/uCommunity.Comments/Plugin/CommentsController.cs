using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uCommunity.Core.Extentions;
using uCommunity.Core.User;
using Umbraco.Web.Mvc;

namespace uCommunity.Comments
{
    public class CommentsController : SurfaceController
    {
        private readonly ICommentsService _commentsService;
        private readonly IIntranetUserService _intranetUserService;

        public CommentsController(
            ICommentsService commentsService,
            IIntranetUserService intranetUserService)
        {
            _commentsService = commentsService;
            _intranetUserService = intranetUserService;
        }

        [HttpPost]
        public PartialViewResult Add(CommentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return OverView(model.ActivityId);
            }

            _commentsService.Create(_intranetUserService.GetCurrentUser().Id, model.ActivityId, model.Text, model.ParentId);
            return OverView(model.ActivityId);
        }

        [HttpPut]
        public PartialViewResult Edit(CommentEditModel model)
        {
            var comment = _commentsService.Get(model.Id);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;

            if (!_commentsService.CanEdit(comment, currentUserId))
            {
                return OverView(comment.ActivityId);
            }

            if (!ModelState.IsValid)
            {
                return OverView(comment.ActivityId);
            }

            comment = _commentsService.Update(model.Id, model.Text);
            return OverView(comment.ActivityId);
        }

        [HttpDelete]
        public PartialViewResult Delete(Guid id)
        {
            var comment = _commentsService.Get(id);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;

            if (_commentsService.CanDelete(comment, currentUserId))
            {
                _commentsService.Delete(id);
            }

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

        public PartialViewResult ListView(Guid activityId)
        {
            var model = new CommentsListModel
            {
                Comments = GetCommentViews(activityId)
            };

            return PartialView("~/App_Plugins/Comments/View/CommentsListView.cshtml", model);
        }

        public PartialViewResult OverView(Guid activityId)
        {
            return PartialView("~/App_Plugins/Comments/View/CommentsOverView.cshtml", GetCommentsOverview(activityId));
        }

        public PartialViewResult PreView(Guid activityId, string link)
        {
            var model = new CommentPreviewModel
            {
                Count = _commentsService.GetCount(activityId),
                Link = $"{link}#{GetOverviewElementId(activityId)}"
            };
            return PartialView("~/App_Plugins/Comments/View/CommentsPreView.cshtml", model);
        }

        public PartialViewResult View(CommentViewModel comment)
        {
            return PartialView("~/App_Plugins/Comments/View/CommentsView.cshtml", comment);
        }

        private CommentsOverviewModel GetCommentsOverview(Guid activityId)
        {
            var model = new CommentsOverviewModel
            {
                ActivityId = activityId,
                Comments = GetCommentViews(activityId),
                ElementId = GetOverviewElementId(activityId)
            };
            return model;
        }

        private IEnumerable<CommentViewModel> GetCommentViews(Guid activityId)
        {
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            var comments = _commentsService.GetMany(activityId).OrderBy(c => c.CreatedDate).ToList();
            var creators = _intranetUserService.GetAll().ToList();
            var replies = comments.FindAll(_commentsService.IsReply);

            foreach (var comment in comments.FindAll(c => !_commentsService.IsReply(c)))
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
            //model.Photo = creator?.Photo;
            model.ElementOverviewId = GetOverviewElementId(comment.ActivityId);
            return model;
        }

        private static string GetOverviewElementId(Guid activityId)
        {
            return $"js-comments-overview-{activityId}";
        }
    }
}