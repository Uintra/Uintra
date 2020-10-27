using System;
using System.Threading.Tasks;
using UBaseline.Shared.Node;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Comments.Links
{
    public interface ICommentLinkHelper
    {
        UintraLinkModel GetDetailsUrlWithComment(Guid activityId, Guid commentId);
        UintraLinkModel GetDetailsUrlWithComment(INodeModel content, Guid commentId);
        Task<UintraLinkModel> GetDetailsUrlWithCommentAsync(Guid activityId, Guid commentId);
    }
}
