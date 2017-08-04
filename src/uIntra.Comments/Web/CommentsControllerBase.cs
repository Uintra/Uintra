using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR.Infrastructure;
using uIntra.Core.Activity;
using uIntra.Core.Controls.LightboxGallery;
using uIntra.Core.Extentions;
using uIntra.Core.Media;
using uIntra.Core.User;
using Umbraco.Web.Mvc;

namespace uIntra.Comments.Web
{
    public abstract class CommentsControllerBase : SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Comments/View/CommentsOverView.cshtml";
        protected virtual string PreviewViewPath { get; } = "~/App_Plugins/Comments/View/CommentsPreView.cshtml";
        protected virtual string EditViewPath { get; } = "~/App_Plugins/Comments/View/CommentsEditView.cshtml";
        protected virtual string CreateViewPath { get; } = "~/App_Plugins/Comments/View/CommentsCreateView.cshtml";

        private readonly ICommentsService _commentsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IIntranetUserContentHelper _intranetUserContentHelper;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IMediaHelper _mediaHelper;

        protected CommentsControllerBase(
            ICommentsService commentsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory,
            IIntranetUserContentHelper intranetUserContentHelper, 
            IIntranetMediaService intranetMediaService, 
            IMediaHelper mediaHelper)
        {
            _commentsService = commentsService;
            _intranetUserService = intranetUserService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _intranetUserContentHelper = intranetUserContentHelper;
            _intranetMediaService = intranetMediaService;
            _mediaHelper = mediaHelper;
        }
        
        [HttpPost]
        public virtual PartialViewResult Add(CommentCreateModel model)
        {
            FillProfileLink();
            if (!ModelState.IsValid)
            {
                return OverView(model.ActivityId);
            }
            var service = _activitiesServiceFactory.GetService<ICommentableService>(model.ActivityId);
            var comment = service.CreateComment(_intranetUserService.GetCurrentUser().Id, model.ActivityId, model.Text, model.ParentId);

            CreateMedia(comment.Id, model);

            OnCommentCreated(comment);

            return OverView(model.ActivityId);
        }

        [HttpPut]
        public virtual PartialViewResult Edit(CommentEditModel model)
        {
            FillProfileLink();
            var comment = _commentsService.Get(model.Id);

            if (!ModelState.IsValid || !_commentsService.CanEdit(comment, _intranetUserService.GetCurrentUser().Id))
            {
                return OverView(model.Id);
            }

            var service = _activitiesServiceFactory.GetService<ICommentableService>(comment.ActivityId);
            service.UpdateComment(model.Id, model.Text);

            UpdateCommentsMedia(comment.Id, model);
 
            OnCommentEdited(comment);
            return OverView(comment.ActivityId);
        }

        [HttpDelete]
        public virtual PartialViewResult Delete(Guid id)
        {
            FillProfileLink();
            var comment = _commentsService.Get(id);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;

            if (!_commentsService.CanDelete(comment, currentUserId))
            {
                return OverView(comment.ActivityId);
            }

            var service = _activitiesServiceFactory.GetService<ICommentableService>(comment.ActivityId);
            service.DeleteComment(id);
            _intranetMediaService.Delete(id);

            return OverView(comment.ActivityId);
        }

        public virtual PartialViewResult CreateView(Guid activityId)
        {
            var mediaSettings = _commentsService.GetMediaSettings();
            var model = new CommentCreateModel
            {
                ActivityId = activityId,
                UpdateElementId = GetOverviewElementId(activityId),
                MediaRootId = mediaSettings.MediaRootId,
                AllowedMediaExtentions = mediaSettings.AllowedMediaExtentions,
            };
            return PartialView(CreateViewPath, model);
        }

        public virtual PartialViewResult EditView(Guid id, string updateElementId)
        {
            var comment = _commentsService.Get(id);
            var model = new CommentEditModel
            {
                Id = id,
                Text = comment.Text,
                UpdateElementId = updateElementId
            };
            return PartialView(EditViewPath, model);
        }

        public virtual PartialViewResult OverView(ICommentable commentsInfo)
        {
            return OverView(commentsInfo.Id, commentsInfo.Comments);
        }

        public virtual PartialViewResult PreView(Guid activityId, string link)
        {
            var model = new CommentPreviewModel
            {
                Count = _commentsService.GetCount(activityId),
                Link = $"{link}#{GetOverviewElementId(activityId)}"
            };
            return PartialView(PreviewViewPath, model);
        }

        protected virtual void FillProfileLink()
        {
            var profilePageUrl = _intranetUserContentHelper.GetProfilePage().Url;
            ViewData.SetProfilePageUrl(profilePageUrl);
        }

        protected virtual PartialViewResult OverView(Guid activityId)
        {
            return OverView(activityId, _commentsService.GetMany(activityId));
        }

        protected virtual PartialViewResult OverView(Guid activityId, IEnumerable<Comment> comments)
        {
            var model = new CommentsOverviewModel
            {
                ActivityId = activityId,
                Comments = GetCommentViews(comments),
                ElementId = GetOverviewElementId(activityId)
            };

            return PartialView(OverviewViewPath, model);
        }

        protected virtual IEnumerable<CommentViewModel> GetCommentViews(IEnumerable<Comment> comments)
        {
            comments = comments.OrderBy(c => c.CreatedDate);
            var commentsList = comments as List<Comment> ?? comments.ToList();
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            var creators = _intranetUserService.GetAll().ToList();
            var replies = commentsList.FindAll(_commentsService.IsReply);
            var mediaSettings = _commentsService.GetMediaSettings();
            foreach (var comment in commentsList.FindAll(c => !_commentsService.IsReply(c)))
            {
                var model = GetCommentView(comment, currentUserId, creators.SingleOrDefault(c => c.Id == comment.UserId), mediaSettings);
                model.MediaIds = _intranetMediaService.GetEntityMediaString(comment.Id);
                var commentReplies = replies.FindAll(reply => reply.ParentId == model.Id);
                model.Replies = commentReplies.Select(reply => GetCommentView(reply, currentUserId, creators.SingleOrDefault(c => c.Id == reply.UserId), mediaSettings));
                yield return model;
            }
        }

        protected virtual CommentViewModel GetCommentView(Comment comment, Guid currentUserId, IIntranetUser creator, MediaSettings mediaSettings)
        {
            var model = comment.Map<CommentViewModel>();
            model.ModifyDate = _commentsService.WasChanged(comment) ? comment.ModifyDate : default(DateTime?);
            model.CanEdit = _commentsService.CanEdit(comment, currentUserId);
            model.CanDelete = _commentsService.CanDelete(comment, currentUserId);
            model.Creator = creator;
            model.ElementOverviewId = GetOverviewElementId(comment.ActivityId);
            model.CommentViewId = _commentsService.GetCommentViewId(comment.Id);
            model.MediaIds = _intranetMediaService.GetEntityMediaString(comment.Id);
            model.MediaSettings = mediaSettings;
            return model;
        }

        protected virtual string GetOverviewElementId(Guid activityId)
        {
            return $"js-comments-overview-{activityId}";
        }

        protected virtual void UpdateCommentsMedia(Guid entityId, CommentEditModel model)
        {
            var existingMediaIds = _intranetMediaService.GetEntityMedia(model.Id);

            if (model.NewMedia.IsNotNullOrEmpty())
            {
                var newMediaIds = _mediaHelper.CreateMedia(model);
                var resultMediaIds = existingMediaIds.Concat(newMediaIds);
                _intranetMediaService.Update(entityId, resultMediaIds.JoinToString());
            }
        }

        protected virtual void CreateMedia(Guid entityId, IContentWithMediaCreateEditModel model)
        {
            if (model.NewMedia.IsNotNullOrEmpty())
            {
                var resultMediaIds = _mediaHelper.CreateMedia(model);
                _intranetMediaService.Create(entityId, resultMediaIds.JoinToString());
            }
        }

        protected virtual void DeleteMedia(Guid entityId)
        {
            _intranetMediaService.Delete(entityId);
        }

        protected virtual void OnCommentCreated(Comment comment)
        {

        }

        protected virtual void OnCommentEdited(Comment comment)
        {
        }
    }
}
