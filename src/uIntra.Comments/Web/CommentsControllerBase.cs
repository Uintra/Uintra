using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Compent.CommandBus;
using Uintra.Comments.CommandBus;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.Context.Extensions;
using Uintra.Core.Extensions;
using Uintra.Core.LinkPreview;
using Uintra.Core.Links;
using Uintra.Core.User;
using Umbraco.Web;
using static Uintra.Core.Context.ContextBuildActionType;

namespace Uintra.Comments.Web
{
    [TrackContext]
    public abstract class CommentsControllerBase : ContextController
    {
        public override Enum ControllerContextType { get; } = ContextType.Comment;

        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Comments/View/CommentsOverView.cshtml";
        protected virtual string PreviewViewPath { get; } = "~/App_Plugins/Comments/View/CommentsPreView.cshtml";
        protected virtual string EditViewPath { get; } = "~/App_Plugins/Comments/View/CommentsEditView.cshtml";
        protected virtual string CreateViewPath { get; } = "~/App_Plugins/Comments/View/CommentsCreateView.cshtml";
        protected virtual string ViewPath { get; } = "~/App_Plugins/Comments/View/CommentsView.cshtml";

        private readonly ICommentsService _commentsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        protected CommentsControllerBase(
            ICommentsService commentsService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            IContextTypeProvider contextTypeProvider,
            ICommandPublisher commandPublisher,
            IActivitiesServiceFactory activitiesServiceFactory)
            : base(contextTypeProvider)
        {
            _commentsService = commentsService;
            _intranetMemberService = intranetMemberService;
            _profileLinkProvider = profileLinkProvider;
            _commandPublisher = commandPublisher;
            _activitiesServiceFactory = activitiesServiceFactory;

            ContextBuildActionType = Erasure;
        }

        [HttpPost]
        [ContextAction(ContextBuildActionType.Add)]
        public virtual PartialViewResult Add(CommentCreateModel model)
        {
            var commentsTarget = FullContext.GetCommentsTarget();
            var targetEntityId = commentsTarget.EntityId.Value;

            if (!ModelState.IsValid)
            {
                return OverView(targetEntityId);
            }

            var createDto = MapToCreateDto(model, targetEntityId);
            var command = new AddCommentCommand(FullContext, createDto);
            _commandPublisher.Publish(command);

            OnCommentCreated(createDto.Id);

            switch (commentsTarget.Type.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(targetEntityId);
                    return OverView(activityCommentsInfo);
                default:
                    return OverView(targetEntityId);
            }
        }

        [HttpPut]
        [ContextAction(Erasure)]
        public virtual PartialViewResult Edit(CommentEditModel model)
        {
            var editCommentId = FullContext.Value.EntityId.Value;
            var commentsTarget = FullContext.GetCommentsTarget();
            var targetEntityId = commentsTarget.EntityId.Value;

            if (!ModelState.IsValid)
            {
                return OverView(editCommentId);
            }

            var comment = _commentsService.Get(editCommentId);
            if (!_commentsService.CanEdit(comment, _intranetMemberService.GetCurrentMemberId()))
            {
                return OverView(editCommentId);
            }

            var editDto = MapToEditDto(model, editCommentId);
            var command = new EditCommentCommand(FullContext, editDto);
            _commandPublisher.Publish(command);

            OnCommentEdited(editCommentId);

            switch (commentsTarget.Type.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(targetEntityId);
                    return OverView(activityCommentsInfo);
                default:
                    return OverView(comment.ActivityId);
            }
        }

        [HttpDelete]
        [ContextAction(Remove)]
        public virtual PartialViewResult Delete()
        {
            var deleteCommentId = FullContext.Value.EntityId.Value;
            var commentsTarget = FullContext.GetCommentsTarget();
            var targetEntityId = commentsTarget.EntityId.Value;

            var comment = _commentsService.Get(deleteCommentId);
            if (!_commentsService.CanDelete(comment, _intranetMemberService.GetCurrentMemberId()))
            {
                return OverView(comment.ActivityId);
            }

            var command = new RemoveCommentCommand(FullContext, deleteCommentId);
            _commandPublisher.Publish(command);

            switch (commentsTarget.Type.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(targetEntityId);
                    return OverView(activityCommentsInfo);
                default:
                    return OverView(comment.ActivityId);
            }
        }

        public virtual PartialViewResult ContentComments()
        {
            var guid = CurrentPage.GetKey();
            return OverView(guid, _commentsService.GetMany(guid));
        }

        public virtual PartialViewResult CreateView(Guid activityId)
        {
            var model = new CommentCreateModel { UpdateElementId = GetOverviewElementId(activityId) };
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
            var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
            var model = new CommentPreviewModel
            {
                Count = _commentsService.GetCount(activityId),
                Link = $"{link}#comments",
                IsReadOnly = isReadOnly,
                IsExistsUserComment = _commentsService.IsExistsUserComment(activityId, currentMemberId)
            };
            return PartialView(PreviewViewPath, model);
        }

        public virtual PartialViewResult CommentsView(CommentViewModel viewModel)
        {
            AddEntityIdentityForContext(viewModel.Id);
            return PartialView(ViewPath, viewModel);
        }

        protected virtual PartialViewResult OverView(Guid activityId)
        {
            return OverView(activityId, _commentsService.GetMany(activityId));
        }

        protected virtual PartialViewResult OverView(Guid activityId, IEnumerable<CommentModel> comments,
            bool isReadOnly = false)
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
            var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
            var creators = _intranetMemberService.GetAll().ToList();
            var replies = commentsList.FindAll(_commentsService.IsReply);

            foreach (var comment in commentsList.FindAll(c => !_commentsService.IsReply(c)))
            {
                var model = GetCommentView(comment, currentMemberId,
                    creators.SingleOrDefault(c => c.Id == comment.UserId));
                var commentReplies = replies.FindAll(reply => reply.ParentId == model.Id);
                model.Replies = commentReplies.Select(reply =>
                    GetCommentView(reply, currentMemberId, creators.SingleOrDefault(c => c.Id == reply.UserId)));
                yield return model;
            }
        }

        protected virtual CommentViewModel GetCommentView(CommentModel comment, Guid currentMemberId,
            IIntranetMember creator)
        {
            var model = comment.Map<CommentViewModel>();
            model.ModifyDate = _commentsService.WasChanged(comment) ? comment.ModifyDate : default(DateTime?);
            model.CanEdit = _commentsService.CanEdit(comment, currentMemberId);
            model.CanDelete = _commentsService.CanDelete(comment, currentMemberId);
            model.Creator = creator.Map<MemberViewModel>();
            model.ElementOverviewId = GetOverviewElementId(comment.ActivityId);
            model.CommentViewId = _commentsService.GetCommentViewId(comment.Id);
            model.CreatorProfileUrl = _profileLinkProvider.GetProfileLink(creator);
            model.LinkPreview = comment.LinkPreview.Map<LinkPreviewViewModel>();
            return model;
        }

        protected virtual ICommentable GetActivityComments(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);
            return (ICommentable)service.Get(activityId);
        }

        protected virtual CommentCreateDto MapToCreateDto(CommentCreateModel createModel, Guid activityId)
        {
            var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
            var dto = new CommentCreateDto(
                Guid.NewGuid(),
                currentMemberId,
                activityId,
                createModel.Text,
                createModel.ParentId,
                createModel.LinkPreviewId);

            return dto;
        }

        protected virtual CommentEditDto MapToEditDto(CommentEditModel editModel, Guid commentId)
        {
            return new CommentEditDto(commentId, editModel.Text, editModel.LinkPreviewId);
        }

        protected virtual string GetOverviewElementId(Guid activityId)
        {
            return $"js-comments-overview-{activityId}";
        }

        protected virtual void OnCommentCreated(Guid commentId)
        {
        }

        protected virtual void OnCommentEdited(Guid commentId)
        {
        }
    }
}
