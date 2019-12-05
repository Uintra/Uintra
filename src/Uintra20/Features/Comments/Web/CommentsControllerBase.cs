using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Compent.CommandBus;
using Compent.Shared.Extensions;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.CommandBus.Commands;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.LinkPreview.Models;
using Uintra20.Features.Links;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Web.WebApi;

namespace Uintra20.Features.Comments.Web
{
    public abstract class CommentsControllerBase : UmbracoApiController
    {
        protected virtual string OverviewViewPath { get; } = "~/App_Plugins/Comments/View/CommentsOverView.cshtml";
        protected virtual string PreviewViewPath { get; } = "~/App_Plugins/Comments/View/CommentsPreView.cshtml";
        protected virtual string EditViewPath { get; } = "~/App_Plugins/Comments/View/CommentsEditView.cshtml";
        protected virtual string CreateViewPath { get; } = "~/App_Plugins/Comments/View/CommentsCreateView.cshtml";
        protected virtual string ViewPath { get; } = "~/App_Plugins/Comments/View/CommentsView.cshtml";

        private readonly ICommentsService _commentsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        protected CommentsControllerBase(
            ICommentsService commentsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            ICommandPublisher commandPublisher,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _commentsService = commentsService;
            _intranetMemberService = intranetMemberService;
            _profileLinkProvider = profileLinkProvider;
            _commandPublisher = commandPublisher;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        [HttpPost]
        public virtual CommentsOverviewModel Add(Guid entityId, Enum entityType, CommentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return OverView(entityId);
            }

            var createDto = MapToCreateDto(model, entityId);
            var command = new AddCommentCommand(entityId, entityType, createDto);
            _commandPublisher.Publish(command);

            OnCommentCreated(createDto.Id);

            switch (entityType.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(entityId);
                    return OverView(activityCommentsInfo);
                default:
                    return OverView(entityId);
            }
        }

        [HttpPut]
        public virtual CommentsOverviewModel Edit(Guid entityId, Enum entityType, CommentEditModel model)
        {
            var editCommentId = model.Id;

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
            var command = new EditCommentCommand(entityId, entityType, editDto);
            _commandPublisher.Publish(command);

            OnCommentEdited(editCommentId);

            switch (entityType.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(entityId);
                    return OverView(activityCommentsInfo);
                default:
                    return OverView(comment.ActivityId);
            }
        }

        [HttpDelete]
        public virtual CommentsOverviewModel Delete(Guid targetId, Enum targetType, Guid commentId)
        {
            var comment = _commentsService.Get(commentId);
            if (!_commentsService.CanDelete(comment, _intranetMemberService.GetCurrentMemberId()))
            {
                return OverView(comment.ActivityId);
            }

            var command = new RemoveCommentCommand(targetId, targetType, commentId);
            _commandPublisher.Publish(command);

            switch (targetType.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(targetId);
                    return OverView(activityCommentsInfo);
                default:
                    return OverView(comment.ActivityId);
            }
        }

        [HttpGet]
        public virtual CommentsOverviewModel ContentComments(Guid pageId)
        {
            return OverView(pageId, _commentsService.GetMany(pageId));
        }

        [HttpGet]
        public virtual CommentsOverviewModel OverView(ICommentable commentsInfo)
        {
            return OverView(commentsInfo.Id, commentsInfo.Comments, commentsInfo.IsReadOnly);
        }

        [HttpGet]
        public virtual CommentPreviewModel PreView(Guid activityId, string link, bool isReadOnly)
        {
            var currentMemberId = _intranetMemberService.GetCurrentMember().Id;
            var model = new CommentPreviewModel
            {
                Count = _commentsService.GetCount(activityId),
                Link = $"{link}#comments",
                IsReadOnly = isReadOnly,
                IsExistsUserComment = _commentsService.IsExistsUserComment(activityId, currentMemberId)
            };

            return model;
        }

        protected virtual CommentsOverviewModel OverView(Guid activityId)
        {
            return OverView(activityId, _commentsService.GetMany(activityId));
        }

        protected virtual CommentsOverviewModel OverView(Guid activityId, IEnumerable<CommentModel> comments,
            bool isReadOnly = false)
        {
            var model = new CommentsOverviewModel
            {
                ActivityId = activityId,
                Comments = GetCommentViews(comments),
                ElementId = GetOverviewElementId(activityId),
                IsReadOnly = isReadOnly
            };

            return model;
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