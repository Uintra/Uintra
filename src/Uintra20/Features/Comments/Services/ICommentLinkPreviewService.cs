using System;
using System.Threading.Tasks;
using Uintra20.Features.LinkPreview.Models;

namespace Uintra20.Features.Comments.Services
{
    public interface ICommentLinkPreviewService
    {
        Task<LinkPreviewModel> GetCommentsLinkPreviewAsync(Guid commentId);
        Task RemovePreviewRelationsAsync(Guid commentId);
        Task AddLinkPreviewAsync(Guid commentId, int previewId);
        Task UpdateLinkPreviewAsync(Guid commentId, int previewId);

        LinkPreviewModel GetCommentsLinkPreview(Guid commentId);
        void RemovePreviewRelations(Guid commentId);
        void AddLinkPreview(Guid commentId, int previewId);
        void UpdateLinkPreview(Guid commentId, int previewId);
    }
}
