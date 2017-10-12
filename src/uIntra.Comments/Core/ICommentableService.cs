using System;
using uIntra.Core.Activity;

namespace uIntra.Comments
{
    public interface ICommentableService : ITypedService
    {
        Comment CreateComment(Guid userId, Guid activityId, string text, Guid? parentId);

        void UpdateComment(Guid id, string text);

        void DeleteComment(Guid id);
        
        ICommentable GetCommentsInfo(Guid activityId);
    }
}
