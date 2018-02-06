using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Extensions;
using uIntra.Core.LinkPreview;
using uIntra.Core.Links;
using uIntra.Core.PagePromotion;
using uIntra.Core.User;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace uIntra.Comments.Web
{
    public abstract class CommentsControllerBase : SurfaceController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Comments/View/CommentsOverView.cshtml";
        protected virtual string PreviewViewPath { get; } = "~/App_Plugins/Comments/View/CommentsPreView.cshtml";
        protected virtual string EditViewPath { get; } = "~/App_Plugins/Comments/View/CommentsEditView.cshtml";
        protected virtual string CreateViewPath { get; } = "~/App_Plugins/Comments/View/CommentsCreateView.cshtml";
        protected virtual string ViewPath { get; } = "~/App_Plugins/Comments/View/CommentsView.cshtml";

        private readonly ICommentableService _customCommentableService;
        private readonly ICommentsService _commentsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IUmbracoContentHelper _umbracoContentHelper;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly UmbracoHelper _umbracoHelper;

        protected CommentsControllerBase(
            ICommentsService commentsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory,
            ICommentableService customCommentableService,
            IUmbracoContentHelper umbracoContentHelper,
            IProfileLinkProvider profileLinkProvider,
            UmbracoHelper umbracoHelper)
        {
            _commentsService = commentsService;
            _intranetUserService = intranetUserService;
            _activitiesServiceFactory = activitiesServiceFactory;
            _customCommentableService = customCommentableService;
            _umbracoContentHelper = umbracoContentHelper;
            _profileLinkProvider = profileLinkProvider;
            _umbracoHelper = umbracoHelper;
        }

        [HttpPost]
        public virtual PartialViewResult Add(CommentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return OverView(model.ActivityId);
            }

            var createDto = Map(model);

            if (IsForPagePromotion(model.ActivityId)) return AddActivityComment(createDto);

            if (_umbracoContentHelper.IsForContentPage(model.ActivityId))
            {
                _customCommentableService.CreateComment(createDto);
                return OverView(model.ActivityId);
            }

            return AddActivityComment(createDto);
        }

        [HttpPut]
        public virtual PartialViewResult Edit(CommentEditModel model)
        {
            var comment = _commentsService.Get(model.Id);

            if (!ModelState.IsValid || !_commentsService.CanEdit(comment, _intranetUserService.GetCurrentUser().Id))
            {
                return OverView(model.Id);
            }

            if (IsForPagePromotion(comment.ActivityId)) return EditActivityComment(model, comment);

            if (_umbracoContentHelper.IsForContentPage(comment.ActivityId))
            {
                _customCommentableService.UpdateComment(model.Id, model.Text);
                return OverView(comment.ActivityId);
            }

            return EditActivityComment(model, comment);
        }

        [HttpDelete]
        public virtual PartialViewResult Delete(Guid id)
        {
            var comment = _commentsService.Get(id);
            var currentUserId = _intranetUserService.GetCurrentUser().Id;

            if (!_commentsService.CanDelete(comment, currentUserId))
            {
                return OverView(comment.ActivityId);
            }

            _commentsService.RemovePreviewRelations(id);

            if (IsForPagePromotion(comment.ActivityId)) return RemoveActivityComment(comment);

            if (_umbracoContentHelper.IsForContentPage(comment.ActivityId))
            {
                _customCommentableService.DeleteComment(id);
                return OverView(comment.ActivityId);
            }

            return RemoveActivityComment(comment);
        }

        public virtual PartialViewResult ContentComments()
        {
            var guid = CurrentPage.GetGuidKey();
            return OverView(guid, _commentsService.GetMany(guid));
        }

        public virtual PartialViewResult CreateView(Guid activityId)
        {
            var model = new CommentCreateModel
            {
                ActivityId = activityId,
                UpdateElementId = GetOverviewElementId(activityId)
            };
            return PartialView(CreateViewPath, model);
        }

        public virtual PartialViewResult EditView(Guid id, string updateElementId)
        {
            var comment = _commentsService.Get(id);
            var model = new CommentEditViewModel
            {
                Id = id,
                Text = comment.Text,
                UpdateElementId = updateElementId
            };
            return PartialView(EditViewPath, model);
        }

        public virtual PartialViewResult OverView(ICommentable commentsInfo)
        {
            return OverView(commentsInfo.Id, commentsInfo.Comments, commentsInfo.IsReadOnly);
        }

        public virtual PartialViewResult PreView(Guid activityId, string link, bool isReadOnly)
        {
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            var model = new CommentPreviewModel
            {
                Count = _commentsService.GetCount(activityId),
                Link = $"{link}#{GetOverviewElementId(activityId)}",
                IsReadOnly = isReadOnly,
                IsExistsUserComment = _commentsService.IsExistsUserComment(activityId, currentUserId)
            };
            return PartialView(PreviewViewPath, model);
        }

        public virtual PartialViewResult CommentsView(CommentViewModel viewModel)
        {
            return PartialView(ViewPath, viewModel);
        }

        protected virtual PartialViewResult AddActivityComment(CommentCreateDto dto)
        {
            var service = _activitiesServiceFactory.GetService<ICommentableService>(dto.ActivityId);
            var comment = service.CreateComment(dto);
            OnCommentCreated(comment);
            return OverView(dto.ActivityId);
        }

        protected virtual PartialViewResult EditActivityComment(CommentEditModel model, CommentModel comment)
        {
            var service = _activitiesServiceFactory.GetService<ICommentableService>(comment.ActivityId);
            service.UpdateComment(model.Id, model.Text);
            OnCommentEdited(comment);
            return OverView(comment.ActivityId);
        }

        protected virtual PartialViewResult RemoveActivityComment(CommentModel comment)
        {
            var service = _activitiesServiceFactory.GetService<ICommentableService>(comment.ActivityId);
            service.DeleteComment(comment.Id);

            return OverView(comment.ActivityId);
        }

        protected virtual PartialViewResult OverView(Guid activityId)
        {
            return OverView(activityId, _commentsService.GetMany(activityId));
        }

        protected virtual PartialViewResult OverView(Guid activityId, IEnumerable<CommentModel> comments, bool isReadOnly = false)
        {
            var model = new CommentsOverviewModel
            {
                ActivityId = activityId,
                Comments = GetCommentViews(comments),
                ElementId = GetOverviewElementId(activityId),
                IsReadOnly = isReadOnly
            };

            return PartialView(OverviewViewPath, model);
        }

        protected virtual IEnumerable<CommentViewModel> GetCommentViews(IEnumerable<CommentModel> comments)
        {
            comments = comments.OrderBy(c => c.CreatedDate);
            var commentsList = comments as List<CommentModel> ?? comments.ToList();
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

        protected virtual CommentViewModel GetCommentView(CommentModel comment, Guid currentUserId, IIntranetUser creator)
        {
            var model = comment.Map<CommentViewModel>();
            model.ModifyDate = _commentsService.WasChanged(comment) ? comment.ModifyDate : default(DateTime?);
            model.CanEdit = _commentsService.CanEdit(comment, currentUserId);
            model.CanDelete = _commentsService.CanDelete(comment, currentUserId);
            model.Creator = creator;
            model.ElementOverviewId = GetOverviewElementId(comment.ActivityId);
            model.CommentViewId = _commentsService.GetCommentViewId(comment.Id);
            model.CreatorProfileUrl = _profileLinkProvider.GetProfileLink(creator);
            model.LinkPreview = comment.LinkPreview.Map<LinkPreviewViewModel>();
            return model;
        }

        protected virtual CommentCreateDto Map(CommentCreateModel createModel)
        {
            var currentUserId = _intranetUserService.GetCurrentUser().Id;
            var dto = new CommentCreateDto(currentUserId,
                createModel.ActivityId,
                createModel.Text,
                createModel.ParentId,
                createModel.LinkPreviewId
            );

            return dto;
        }

        protected virtual bool IsForPagePromotion(Guid entityId)
        {
            var content = _umbracoHelper.TypedContent(entityId);
            return content != null && PagePromotionHelper.IsPagePromotion(content) && PagePromotionHelper.IsPromoted(content);
        }

        protected virtual string GetOverviewElementId(Guid activityId)
        {
            return $"js-comments-overview-{activityId}";
        }

        protected virtual void OnCommentCreated(CommentModel comment)
        {

        }

        protected virtual void OnCommentEdited(CommentModel comment)
        {
        }
    }
}
