using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Compent.CommandBus;
using UBaseline.Core.Controllers;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
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

namespace Uintra20.Features.Comments.Web
{
    public abstract class CommentsControllerBase : UBaselineApiController
    {
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
        public virtual async Task<CommentsOverviewModel> Add([FromBody] CommentCreateModel model)
        {
            var createDto = await MapToCreateDtoAsync(model, model.EntityId);
            var command = new AddCommentCommand(model.EntityId, model.EntityType, createDto);
            _commandPublisher.Publish(command);

            await OnCommentCreatedAsync(createDto.Id);

            switch (model.EntityType.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(model.EntityId);
                    return await OverViewAsync(activityCommentsInfo.Id, activityCommentsInfo.Comments, activityCommentsInfo.IsReadOnly);
                default:
                    return await OverViewAsync(model.EntityId);
            }
        }

        [HttpPut]
        public virtual async Task<CommentsOverviewModel> Edit(Guid entityId, ContextType entityType,
            CommentEditModel model)
        {
            var editCommentId = model.Id;

            var comment = await _commentsService.GetAsync(editCommentId);
            if (!_commentsService.CanEdit(comment, await _intranetMemberService.GetCurrentMemberIdAsync()))
            {
                return await OverViewAsync(editCommentId);
            }

            var editDto = MapToEditDto(model, editCommentId);
            var command = new EditCommentCommand(entityId, entityType, editDto);
            _commandPublisher.Publish(command);

            await OnCommentEditedAsync(editCommentId);

            switch (entityType.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(entityId);
                    return await OverViewAsync(activityCommentsInfo.Id, activityCommentsInfo.Comments, activityCommentsInfo.IsReadOnly);
                default:
                    return await OverViewAsync(comment.ActivityId);
            }
        }

        [HttpDelete]
        public virtual async Task<CommentsOverviewModel> Delete(Guid targetId, ContextType targetType, Guid commentId)
        {
            var comment = await _commentsService.GetAsync(commentId);

            if (!_commentsService.CanDelete(comment, await _intranetMemberService.GetCurrentMemberIdAsync()))
            {
                return await OverViewAsync(comment.ActivityId);
            }

            var command = new RemoveCommentCommand(targetId, targetType, commentId);
            _commandPublisher.Publish(command);

            switch (targetType.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(targetId);
                    return await OverViewAsync(activityCommentsInfo.Id, activityCommentsInfo.Comments, activityCommentsInfo.IsReadOnly);
                default:
                    return await OverViewAsync(comment.ActivityId);
            }
        }

        [HttpGet]
        public virtual async Task<CommentsOverviewModel> ContentComments(Guid pageId)
        {
            return await OverViewAsync(pageId, await _commentsService.GetManyAsync(pageId));
        }

        [HttpGet]
        public virtual async Task<CommentPreviewModel> PreView(Guid activityId, string link, bool isReadOnly)
        {
            var currentMemberId = await _intranetMemberService.GetCurrentMemberIdAsync();
            var model = new CommentPreviewModel
            {
                Count = await _commentsService.GetCountAsync(activityId),
                Link = $"{link}#comments",
                IsReadOnly = isReadOnly,
                IsExistsUserComment = await _commentsService.IsExistsUserCommentAsync(activityId, currentMemberId)
            };

            return model;
        }

        protected virtual CommentsOverviewModel OverView(Guid activityId)
        {
            return OverView(activityId, _commentsService.GetMany(activityId));
        }

        protected virtual async Task<CommentsOverviewModel> OverViewAsync(Guid activityId)
        {
            return await OverViewAsync(activityId, await _commentsService.GetManyAsync(activityId));
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

        protected virtual async Task<CommentsOverviewModel> OverViewAsync(Guid activityId,
            IEnumerable<CommentModel> comments,
            bool isReadOnly = false)
        {
            var model = new CommentsOverviewModel
            {
                ActivityId = activityId,
                Comments = await GetCommentViewsAsync(comments),
                ElementId = GetOverviewElementId(activityId),
                IsReadOnly = isReadOnly
            };

            return model;
        }

        protected virtual IEnumerable<CommentViewModel> GetCommentViews(IEnumerable<CommentModel> comments)
        {
            comments = comments.OrderBy(c => c.CreatedDate);
            var commentsList = comments as List<CommentModel> ?? comments.ToList();
            var currentMemberId = _intranetMemberService.GetCurrentMemberId();
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

        protected virtual async Task<IEnumerable<CommentViewModel>> GetCommentViewsAsync(IEnumerable<CommentModel> comments)
        {
            comments = comments.OrderBy(c => c.CreatedDate);
            var commentsList = comments as List<CommentModel> ?? comments.ToList();
            var currentMemberId = await _intranetMemberService.GetCurrentMemberIdAsync();
            var creators = (await _intranetMemberService.GetAllAsync()).ToList();
            var replies = commentsList.FindAll(_commentsService.IsReply);

            var list = new List<CommentViewModel>();

            foreach (var comment in commentsList.FindAll(c => !_commentsService.IsReply(c)))
            {
                var model = GetCommentView(comment, currentMemberId,
                    creators.SingleOrDefault(c => c.Id == comment.UserId));
                var commentReplies = replies.FindAll(reply => reply.ParentId == model.Id);
                model.Replies = commentReplies.Select(reply =>
                    GetCommentView(reply, currentMemberId, creators.SingleOrDefault(c => c.Id == reply.UserId)));
                list.Add(model);
            }

            return list;
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
            model.CreatorProfileUrl = creator == null ? null : _profileLinkProvider.GetProfileLink(creator);
            model.LinkPreview = comment.LinkPreview.Map<LinkPreviewViewModel>();
            return model;
        }

        protected virtual ICommentable GetActivityComments(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);
            return (ICommentable)service.Get(activityId);
        }

        protected virtual CommentCreateDto MapToCreateDto(CommentCreateModel createModel,
            Guid activityId)
        {
            var currentMemberId = _intranetMemberService.GetCurrentMemberId();
            var dto = new CommentCreateDto(
                Guid.NewGuid(),
                currentMemberId,
                activityId,
                createModel.Text,
                createModel.ParentId,
                createModel.LinkPreviewId);

            return dto;
        }

        protected virtual async Task<CommentCreateDto> MapToCreateDtoAsync(CommentCreateModel createModel,
            Guid activityId)
        {
            var currentMemberId = await _intranetMemberService.GetCurrentMemberIdAsync();
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

        protected virtual async Task OnCommentCreatedAsync(Guid commentId)
        {
        }

        protected virtual async Task OnCommentEditedAsync(Guid commentId)
        {
        }

        protected virtual void OnCommentCreated(Guid commentId)
        {
        }

        protected virtual void OnCommentEdited(Guid commentId)
        {
        }
    }
}