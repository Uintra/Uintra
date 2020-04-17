using System;
using System.Threading.Tasks;

namespace Uintra20.Features.LinkPreview.Services
{
    public interface IActivityLinkPreviewService
    {
        Models.LinkPreview GetActivityLinkPreview(Guid activityId);
        void RemovePreviewRelations(Guid activityId);
        void AddLinkPreview(Guid activityId, int previewId);
        void UpdateLinkPreview(Guid activityId, int previewId);

        Task<Models.LinkPreview> GetActivityLinkPreviewAsync(Guid activityId);
        Task RemovePreviewRelationsAsync(Guid activityId);
        Task AddLinkPreviewAsync(Guid activityId, int previewId);
        Task UpdateLinkPreviewAsync(Guid activityId, int previewId);
    }
}