using System;
using uIntra.Core.Activity;

namespace uIntra.Comments
{
    public interface ICommentableService : ITypedService
    {
        CommentModel CreateComment(CommentDto dto);

        void UpdateComment(Guid id, string text);

        void DeleteComment(Guid id);
        
        ICommentable GetCommentsInfo(Guid activityId);
    }
}