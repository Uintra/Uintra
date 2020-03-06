using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using UBaseline.Core.Node;
using Uintra20.Attributes;
using Uintra20.Core;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.CommandBus.Commands;
using Uintra20.Features.Comments.Helpers;
using Uintra20.Features.Comments.Links;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Notification;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Comments.Controllers
{
    [ThreadCulture]
    [ValidateModel]
    public class CommentsController : UBaselineApiController
    {
        private readonly ICommentsHelper _commentsHelper;
        private readonly ICommentsService _commentsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly ICommandPublisher _commandPublisher;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;
        private readonly IMentionService _mentionService;
        private readonly ICommentLinkHelper _commentLinkHelper;
        private readonly INodeModelService _nodeModelService;
        private readonly IGroupActivityService _groupActivityService;

        public CommentsController(
            ICommentsHelper commentsHelper,
            ICommentsService commentsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ICommandPublisher commandPublisher,
            IActivitiesServiceFactory activitiesServiceFactory,
            IMentionService mentionService,
            ICommentLinkHelper commentLinkHelper,
            IGroupActivityService groupActivityService,
            INodeModelService nodeModelService)
        {
            _commentsHelper = commentsHelper;
            _commentsService = commentsService;
            _intranetMemberService = intranetMemberService;
            _commandPublisher = commandPublisher;
            _activitiesServiceFactory = activitiesServiceFactory;
            _mentionService = mentionService;
            _commentLinkHelper = commentLinkHelper;
            _groupActivityService = groupActivityService;
            _nodeModelService = nodeModelService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Add([FromBody] CommentCreateModel model)
        {
            if (model.EntityType.Is(IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Events))
            {
                var member = await _intranetMemberService.GetCurrentMemberAsync();
                var activityGroupId = _groupActivityService.GetGroupId(model.EntityId);

                if(activityGroupId.HasValue && !member.GroupIds.Contains(activityGroupId.Value))
                {
                    return StatusCode(HttpStatusCode.Forbidden);
                }
            }

            var createDto = await MapToCreateDtoAsync(model, model.EntityId);
            var command = new AddCommentCommand(model.EntityId, model.EntityType, createDto);
            _commandPublisher.Publish(command);

            await OnCommentCreatedAsync(createDto.Id);

            switch (model.EntityType)
            {
                case IntranetEntityTypeEnum type
                    when type.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events):
                    var activityCommentsInfo = GetActivityComments(model.EntityId);
                    return Ok(await _commentsHelper.OverViewAsync(activityCommentsInfo.Id, activityCommentsInfo.Comments, activityCommentsInfo.IsReadOnly));
                default:
                    return Ok(await _commentsHelper.OverViewAsync(model.EntityId));
            }
        }

        [HttpPut]
        public async Task<CommentsOverviewModel> Edit(CommentEditModel model)
        {
            var comment = await _commentsService.GetAsync(model.Id);
            if (!_commentsService.CanEdit(comment, await _intranetMemberService.GetCurrentMemberIdAsync()))
                return await _commentsHelper.OverViewAsync(model.Id);
            

            var editDto = MapToEditDto(model, model.Id);
            var command = new EditCommentCommand(model.EntityId, model.EntityType, editDto);
            _commandPublisher.Publish(command);

            await OnCommentEditedAsync(model.Id);

            switch (model.EntityType)
            {
                case IntranetEntityTypeEnum type
                    when type.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events):
                    var activityCommentsInfo = GetActivityComments(model.EntityId);
                    return await _commentsHelper.OverViewAsync(activityCommentsInfo.Id, activityCommentsInfo.Comments, activityCommentsInfo.IsReadOnly);
                default:
                    return await _commentsHelper.OverViewAsync(comment.ActivityId);
            }
        }

        [HttpDelete]
        public async Task<CommentsOverviewModel> Delete(Guid targetId, IntranetEntityTypeEnum targetType, Guid commentId)
        {
            var comment = await _commentsService.GetAsync(commentId);

            if (!_commentsService.CanDelete(comment, await _intranetMemberService.GetCurrentMemberIdAsync()))
            {
                return await _commentsHelper.OverViewAsync(comment.ActivityId);
            }

            var command = new RemoveCommentCommand(targetId, targetType, commentId);
            _commandPublisher.Publish(command);

            switch (targetType)
            {
                case IntranetEntityTypeEnum type
                    when type.Is(IntranetEntityTypeEnum.News, IntranetEntityTypeEnum.Social, IntranetEntityTypeEnum.Events):
                    var activityCommentsInfo = GetActivityComments(targetId);
                    return await _commentsHelper.OverViewAsync(activityCommentsInfo.Id, activityCommentsInfo.Comments, activityCommentsInfo.IsReadOnly);
                default:
                    return await _commentsHelper.OverViewAsync(comment.ActivityId);
            }
        }

        [HttpGet]
        public async Task<CommentsOverviewModel> ContentComments(Guid pageId)
        {
            return await _commentsHelper.OverViewAsync(pageId, await _commentsService.GetManyAsync(pageId));
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
                var content = _nodeModelService.AsEnumerable().FirstOrDefault(x => x.Key == comment.ActivityId);
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
            var mentionIds = _mentionService.GetMentions(text).ToArray();

            if (mentionIds.Length == 0) 
                return;

            var content = _nodeModelService.AsEnumerable().FirstOrDefault(x => x.Key == comment.ActivityId);
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