using System;
using uIntra.Core.Activity;

namespace uIntra.Comments
{
    public interface ICommentableService : ITypedService
    {
        CommentModel CreateComment(CommentCreateDto dto);

        void UpdateComment(CommentEditDto dto);

        void DeleteComment(Guid id);
        
        ICommentable GetCommentsInfo(Guid activityId);
    }
}