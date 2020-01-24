using System;
using System.Threading.Tasks;
using Uintra20.Features.Links.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Comments.Links
{
    public interface ICommentLinkHelper
    {
        UintraLinkModel GetDetailsUrlWithComment(Guid activityId, Guid commentId);
        UintraLinkModel GetDetailsUrlWithComment(IPublishedContent content, Guid commentId);
        Task<UintraLinkModel> GetDetailsUrlWithCommentAsync(Guid activityId, Guid commentId);
    }
}
