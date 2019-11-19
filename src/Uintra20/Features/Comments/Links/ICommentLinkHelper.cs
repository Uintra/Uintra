using System;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Comments.Links
{
    public interface ICommentLinkHelper
    {
        string GetDetailsUrlWithComment(Guid activityId, Guid commentId);
        string GetDetailsUrlWithComment(IPublishedContent content, Guid commentId);

        Task<string> GetDetailsUrlWithCommentAsync(Guid activityId, Guid commentId);
    }
}
