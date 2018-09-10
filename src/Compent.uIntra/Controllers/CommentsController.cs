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
using Uintra.Users;

namespace Compent.Uintra.Controllers
{
    [ThreadCulture]
    [TrackContext]
    public class CommentsController : CommentsControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IMentionService _mentionService;
        private readonly ICommentLinkHelper _commentLinkHelper;
        protected override string OverviewViewPath { get; } = "~/Views/Comments/CommentsOverView.cshtml";
        protected override string ViewPath { get; } = "~/Views/Comments/CommentsView.cshtml";


        public CommentsController(
            ICommentsService commentsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IProfileLinkProvider profileLinkProvider,
            IContextTypeProvider contextTypeProvider,
            ICommandPublisher commandPublisher,
            IActivitiesServiceFactory activitiesServiceFactory,
            IMentionService mentionService,
            ICommentLinkHelper commentLinkHelper)
            : base(commentsService, intranetUserService, profileLinkProvider, contextTypeProvider, commandPublisher, activitiesServiceFactory)
        {
            _commentsService = commentsService;
            _intranetUserService = intranetUserService;
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
                _mentionService.PreccessMention(new MentionModel()
                {
                    MentionedSourceId = comment.Id,
                    CreatorId = _intranetUserService.GetCurrentUserId(),
                    MentionedUserIds = mentionIds,
                    Title = $"Comment - \"{comment.Text.StripHtml().TrimByWordEnd(50)}\"",
                    Url = content != null ? _commentLinkHelper.GetDetailsUrlWithComment(content, comment.Id) :
                        _commentLinkHelper.GetDetailsUrlWithComment(comment.ActivityId, comment.Id),
                });

            }
        }
    }
}