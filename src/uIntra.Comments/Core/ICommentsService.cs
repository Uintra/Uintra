using System;
using System.Collections.Generic;

namespace uIntra.Comments
{
    public interface ICommentsService
    {
        CommentModel Get(Guid id);

        IEnumerable<CommentModel> GetMany(Guid activityId);

        int GetCount(Guid activityId);

        bool CanEdit(CommentModel comment, Guid editorId);

        bool CanDelete(CommentModel comment, Guid editorId);

        bool WasChanged(CommentModel comment);

        bool IsReply(CommentModel comment);

        CommentModel Create(CommentDto dto);

        CommentModel Update(Guid id, string text);

        void Delete(Guid id);

        void FillComments(ICommentable entity);

        string GetCommentViewId(Guid commentId);

        bool IsExistsUserComment(Guid activityId, Guid userId);

        void SaveLinkPreview(Guid commentId, int previewId);

        void RemovePreviewRelations(Guid commentId);
    }
}