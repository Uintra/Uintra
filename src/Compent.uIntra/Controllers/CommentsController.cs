using System;
using System.Linq;
using Compent.CommandBus;
using Localization.Umbraco.Attributes;
using Uintra.Comments;
using Uintra.Comments.Web;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Core.Context;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Users;

namespace Compent.Uintra.Controllers
{
    [ThreadCulture]
    [TrackContext]
    public class CommentsController : CommentsControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IMentionService _mentionService;
        private readonly ICommentLinkHelper _commentLinkHelper;

        public CommentsController(
            ICommentsService commentsService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IProfileLinkProvider profileLinkProvider,
            IContextTypeProvider contextTypeProvider,
            ICommandPublisher commandPublisher,
            IActivitiesServiceFactory activitiesServiceFactory,
            IMentionService mentionService,
            ICommentLinkHelper commentLinkHelper)
            : base(commentsService, intranetMemberService, profileLinkProvider, contextTypeProvider, commandPublisher, activitiesServiceFactory)
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
                var content = Umbraco.TypedContent(comment.ActivityId);
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