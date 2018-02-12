using System;
using Uintra.Core.LinkPreview;

namespace Uintra.Comments
{
    public interface ICommentLinkPreviewService
    {
        LinkPreview GetCommentsLinkPreview(Guid commentId);
        void RemovePreviewRelations(Guid commentId);
        void AddLinkPreview(Guid commentId, int previewId);
        void UpdateLinkPreview(Guid commentId, int previewId);
    }
}
