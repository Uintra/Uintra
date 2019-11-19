using System;
using System.Threading.Tasks;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Links;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Comments.Links
{
    public class CommentLinkHelper : ICommentLinkHelper
    {
        private readonly IActivityLinkService _linkService;
        private readonly ICommentsService _commentsService;

        public CommentLinkHelper(IActivityLinkService linkService, ICommentsService commentsService)
        {
            _linkService = linkService;
            _commentsService = commentsService;
        }

        public string GetDetailsUrlWithComment(Guid activityId, Guid commentId) =>
            $"{_linkService.GetLinks(activityId).Details}#{_commentsService.GetCommentViewId(commentId)}";

        public string GetDetailsUrlWithComment(IPublishedContent content, Guid commentId) =>
            $"{content.Url}#{_commentsService.GetCommentViewId(commentId)}";

        public async Task<string> GetDetailsUrlWithCommentAsync(Guid activityId, Guid commentId) =>
            $"{(await _linkService.GetLinksAsync(activityId)).Details}#{_commentsService.GetCommentViewId(commentId)}";
    }
}