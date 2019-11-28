using System;
using System.Linq;
using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Comments.Links;
using Uintra20.Features.Comments.Models;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Comments.Web;
using Uintra20.Features.Links;
using Uintra20.Features.Notification;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Controllers
{
    [ThreadCulture]
    public class CommentsController : CommentsControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IMentionService _mentionService;
        private readonly ICommentLinkHelper _commentLinkHelper;

        public CommentsController(
            ICommentsService commentsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            ICommandPublisher commandPublisher,
            IActivitiesServiceFactory activitiesServiceFactory,
            IMentionService mentionService,
            ICommentLinkHelper commentLinkHelper)
            : base(commentsService, intranetMemberService, profileLinkProvider, commandPublisher, activitiesServiceFactory)
        {
            _commentsService = commentsService;
            _intranetMemberService = intranetMemberService;
            _mentionService = mentionService;
            _commentLinkHelper = commentLinkHelper;
        }

        protected override void OnCommentCreated(Guid commentId)
        {
            var comment = _commentsService.Get(commentId);
            ResolveMentions(comment.Text, comment);
        }

        protected override void OnCommentEdited(Guid commentId)
        {
            var comment = _commentsService.Get(commentId);
            ResolveMentions(comment.Text, comment);
        }

        private void ResolveMentions(string text, CommentModel comment)
        {
            var mentionIds = _mentionService.GetMentions(text).ToList();

            if (mentionIds.Any())
            {
                var content = Umbraco.Content(comment.ActivityId);
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