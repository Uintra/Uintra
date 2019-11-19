using System;
using System.Threading.Tasks;

namespace Uintra20.Features.Comments.Services
{
    public interface ICommentLinkPreviewService
    {
        Task<LinkPreview.Models.LinkPreview> GetCommentsLinkPreviewAsync(Guid commentId);
        Task RemovePreviewRelationsAsync(Guid commentId);
        Task AddLinkPreviewAsync(Guid commentId, int previewId);
        Task UpdateLinkPreviewAsync(Guid commentId, int previewId);

        LinkPreview.Models.LinkPreview GetCommentsLinkPreview(Guid commentId);
        void RemovePreviewRelations(Guid commentId);
        void AddLinkPreview(Guid commentId, int previewId);
        void UpdateLinkPreview(Guid commentId, int previewId);
    }
}
