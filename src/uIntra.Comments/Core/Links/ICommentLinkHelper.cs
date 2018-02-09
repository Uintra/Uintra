using System;

namespace Uintra.Comments
{
    public interface ICommentLinkHelper
    {
        string GetDetailsUrlWithComment(Guid activityId, Guid commentId);
    }
}
