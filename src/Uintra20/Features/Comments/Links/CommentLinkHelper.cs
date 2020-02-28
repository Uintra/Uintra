using System;
using System.Threading.Tasks;
using UBaseline.Shared.Node;
using Uintra20.Features.Comments.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Infrastructure.Extensions;

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

        public UintraLinkModel GetDetailsUrlWithComment(Guid activityId, Guid commentId) =>
            $"{_linkService.GetLinks(activityId).Details}#{_commentsService.GetCommentViewId(commentId)}".ToLinkModel();

        public UintraLinkModel GetDetailsUrlWithComment(INodeModel content, Guid commentId) =>
            $"{content.Url}#{_commentsService.GetCommentViewId(commentId)}".ToLinkModel();

        public async Task<UintraLinkModel> GetDetailsUrlWithCommentAsync(Guid activityId, Guid commentId) =>
            $"{(await _linkService.GetLinksAsync(activityId)).Details}#{_commentsService.GetCommentViewId(commentId)}".ToLinkModel();
    }
}