using System;
using Umbraco.Core.Models;

namespace Uintra.Comments
{
    public interface ICommentLinkHelper
    {
        string GetDetailsUrlWithComment(Guid activityId, Guid commentId);
        string GetDetailsUrlWithComment(IPublishedContent content, Guid commentId);
    }
}
