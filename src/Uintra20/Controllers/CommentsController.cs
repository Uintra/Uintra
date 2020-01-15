using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.CommandBus.Commands;
using Uintra20.Features.Comments.Links;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.LinkPreview.Models;
using Uintra20.Features.Links;
using Uintra20.Features.Notification;
using Uintra20.Infrastructure.Context;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Web;

namespace Uintra20.Controllers
{
    [ThreadCulture]
    [ValidateModel]
    public class CommentsController : UBaselineApiController
    {
        private readonly ICommentsService _commentsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IProfileLinkProvider _profileLinkProvider;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IMentionService _mentionService;
        private readonly ICommentLinkHelper _commentLinkHelper;
        private readonly UmbracoHelper _umbracoHelper;

        public CommentsController(
            ICommentsService commentsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            ICommandPublisher commandPublisher,
            IActivitiesServiceFactory activitiesServiceFactory,
            IMentionService mentionService,
            ICommentLinkHelper commentLinkHelper,
            UmbracoHelper umbracoHelper)
        {
            _commentsService = commentsService;
            _intranetMemberService = intranetMemberService;
            _profileLinkProvider = profileLinkProvider;
            _commandPublisher = commandPublisher;
            _activitiesServiceFactory = activitiesServiceFactory;
            _mentionService = mentionService;
            _commentLinkHelper = commentLinkHelper;
            _umbracoHelper = umbracoHelper;
        }

        [HttpPost]
        public async Task<CommentsOverviewModel> Add([FromBody] CommentCreateModel model)
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
        public async Task<CommentsOverviewModel> Edit(CommentEditModel model)
        {
            var editCommentId = model.Id;

            var comment = await _commentsService.GetAsync(editCommentId);
            if (!_commentsService.CanEdit(comment, await _intranetMemberService.GetCurrentMemberIdAsync()))
            {
                return await OverViewAsync(editCommentId);
            }

            var editDto = MapToEditDto(model, editCommentId);
            var command = new EditCommentCommand(model.EntityId, model.EntityType, editDto);
            _commandPublisher.Publish(command);

            await OnCommentEditedAsync(editCommentId);

            switch (model.EntityType.ToInt())
            {
                case int type
                    when ContextExtensions.HasFlagScalar(type, ContextType.Activity | ContextType.PagePromotion):
                    var activityCommentsInfo = GetActivityComments(model.EntityId);
                    return await OverViewAsync(activityCommentsInfo.Id, activityCommentsInfo.Comments, activityCommentsInfo.IsReadOnly);
                default:
                    return await OverViewAsync(comment.ActivityId);
            }
        }

        [HttpDelete]
        public async Task<CommentsOverviewModel> Delete(Guid targetId, ContextType targetType, Guid commentId)
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
        public async Task<CommentsOverviewModel> ContentComments(Guid pageId)
        {
            return await OverViewAsync(pageId, await _commentsService.GetManyAsync(pageId));
        }

        [HttpGet]
        public async Task<CommentPreviewModel> PreView(Guid activityId, string link, bool isReadOnly)
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
        private CommentEditDto MapToEditDto(CommentEditModel editModel, Guid commentId)
        {
            return new CommentEditDto(commentId, editModel.Text, editModel.LinkPreviewId);
        }
        private async Task<CommentCreateDto> MapToCreateDtoAsync(
            CommentCreateModel createModel,
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
        private ICommentable GetActivityComments(Guid activityId)
        {
            var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityId);

            return (ICommentable)service.Get(activityId);
        }

        private async Task<CommentsOverviewModel> OverViewAsync(Guid activityId)
        {
            return await OverViewAsync(activityId, await _commentsService.GetManyAsync(activityId));
        }

        private async Task<CommentsOverviewModel> OverViewAsync(
            Guid activityId,
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
        private async Task<IEnumerable<CommentViewModel>> GetCommentViewsAsync(IEnumerable<CommentModel> comments)
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
        private CommentViewModel GetCommentView(
            CommentModel comment,
            Guid currentMemberId,
            IIntranetMember creator)
        {
            var model = comment.Map<CommentViewModel>();
            model.ModifyDate = _commentsService.WasChanged(comment) ? comment.ModifyDate.ToDateTimeFormat() : null;
            model.CanEdit = _commentsService.CanEdit(comment, currentMemberId);
            model.CanDelete = _commentsService.CanDelete(comment, currentMemberId);
            model.Creator = creator.Map<MemberViewModel>();
            model.ElementOverviewId = GetOverviewElementId(comment.ActivityId);
            model.CommentViewId = _commentsService.GetCommentViewId(comment.Id);
            model.CreatorProfileUrl = creator == null ? null : _profileLinkProvider.GetProfileLink(creator);
            model.LinkPreview = comment.LinkPreview.Map<LinkPreviewViewModel>();
            return model;
        }
        private string GetOverviewElementId(Guid activityId)
        {
            return $"js-comments-overview-{activityId}";
        }

        private async Task OnCommentCreatedAsync(Guid commentId)
        {
            var comment = await _commentsService.GetAsync(commentId);
            await ResolveMentionsAsync(comment.Text, comment);
        }

        private async Task OnCommentEditedAsync(Guid commentId)
        {
            var comment = await _commentsService.GetAsync(commentId);
            await ResolveMentionsAsync(comment.Text, comment);
        }

        private void OnCommentCreated(Guid commentId)
        {
            var comment = _commentsService.Get(commentId);
            ResolveMentions(comment.Text, comment);
        }

        private void OnCommentEdited(Guid commentId)
        {
            var comment = _commentsService.Get(commentId);
            ResolveMentions(comment.Text, comment);
        }

        private async Task ResolveMentionsAsync(string text, CommentModel comment)
        {
            var mentionIds = _mentionService.GetMentions(text).ToList();

            if (mentionIds.Any())
            {
                var content = _umbracoHelper.Content(comment.ActivityId);
                _mentionService.ProcessMention(new MentionModel
                {
                    MentionedSourceId = comment.Id,
                    CreatorId = await _intranetMemberService.GetCurrentMemberIdAsync(),
                    MentionedUserIds = mentionIds,
                    Title = comment.Text.StripHtml().TrimByWordEnd(50),
                    Url = content != null ? _commentLinkHelper.GetDetailsUrlWithComment(content, comment.Id) :
                        await _commentLinkHelper.GetDetailsUrlWithCommentAsync(comment.ActivityId, comment.Id),
                    ActivityType = CommunicationTypeEnum.CommunicationSettings
                });

            }
        }

        private void ResolveMentions(string text, CommentModel comment)
        {
            var mentionIds = _mentionService.GetMentions(text).ToList();

            if (mentionIds.Any())
            {
                var content = _umbracoHelper.Content(comment.ActivityId);
                _mentionService.ProcessMention(new MentionModel
                {
                    MentionedSourceId = comment.Id,
                    CreatorId = _intranetMemberService.GetCurrentMemberId(),
                    MentionedUserIds = mentionIds,
                    Title = comment.Text.StripHtml().TrimByWordEnd(50),
                    Url = content != null ? _commentLinkHelper.GetDetailsUrlWithComment(content, comment.Id) :
                        _commentLinkHelper.GetDetailsUrlWithComment(comment.ActivityId, comment.Id),
                    ActivityType = CommunicationTypeEnum.CommunicationSettings
                });

            }
        }
    }
}