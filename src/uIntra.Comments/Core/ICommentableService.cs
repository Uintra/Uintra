using System;
using uIntra.Core.Activity;

namespace uIntra.Comments
{
    public interface ICommentableService : ITypedService
    {
        CommentModel CreateComment(CommentCreateDto dto);

        void UpdateComment(Guid id, string text, int? linkPreviewId);

        void DeleteComment(Guid id);
        
        ICommentable GetCommentsInfo(Guid activityId);
    }
}