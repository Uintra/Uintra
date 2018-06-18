using System;

namespace uIntra.Comments
{
    public interface ICommentLinkHelper
    {
        string GetDetailsUrlWithComment(Guid activityId, Guid commentId);
    }
}
