using System;
using System.Threading.Tasks;
using UBaseline.Shared.Node;
using Uintra.Features.Comments.Services;
using Uintra.Features.Links;
using Uintra.Features.Links.Models;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Comments.Links
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

        public UintraLinkModel GetDetailsUrlWithComment(Guid activityId, Guid commentId) =>
            $"{_linkService.GetLinks(activityId).Details}&commentId={_commentsService.GetCommentViewId(commentId)}".ToLinkModel();

        public UintraLinkModel GetDetailsUrlWithComment(INodeModel content, Guid commentId) =>
            $"{content.Url}{(content.Url.Contains("?") ? "&" : "?")}commentId={_commentsService.GetCommentViewId(commentId)}".ToLinkModel();

        public async Task<UintraLinkModel> GetDetailsUrlWithCommentAsync(Guid activityId, Guid commentId) =>
            $"{(await _linkService.GetLinksAsync(activityId)).Details}&commentId={_commentsService.GetCommentViewId(commentId)}".ToLinkModel();
    }
}