using System;
using uIntra.Core.Links;

namespace uIntra.Comments
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
    }
}