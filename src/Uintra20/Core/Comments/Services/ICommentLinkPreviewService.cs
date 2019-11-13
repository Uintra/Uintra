using System;
using System.Threading.Tasks;

namespace Uintra20.Core.Comments
{
    public interface ICommentLinkPreviewService
    {
        Task<LinkPreview.LinkPreview> GetCommentsLinkPreviewAsync(Guid commentId);
        Task RemovePreviewRelationsAsync(Guid commentId);
        Task AddLinkPreviewAsync(Guid commentId, int previewId);
        Task UpdateLinkPreviewAsync(Guid commentId, int previewId);

        LinkPreview.LinkPreview GetCommentsLinkPreview(Guid commentId);
        void RemovePreviewRelations(Guid commentId);
        void AddLinkPreview(Guid commentId, int previewId);
        void UpdateLinkPreview(Guid commentId, int previewId);
    }
}
