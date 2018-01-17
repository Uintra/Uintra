using System;
using uIntra.Core.Activity;

namespace uIntra.Comments
{
    public interface ICommentableService : ITypedService
    {
        CommentModel CreateComment(Guid userId, Guid activityId, string text, Guid? parentId);

        void UpdateComment(Guid id, string text);

        void DeleteComment(Guid id);
        
        ICommentable GetCommentsInfo(Guid activityId);
    }
}
