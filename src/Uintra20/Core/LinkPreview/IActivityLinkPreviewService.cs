using System;
using System.Threading.Tasks;

namespace Uintra20.Core.LinkPreview
{
    public interface IActivityLinkPreviewService
    {
        LinkPreview GetActivityLinkPreview(Guid activityId);
        void RemovePreviewRelations(Guid activityId);
        void AddLinkPreview(Guid activityId, int previewId);
        void UpdateLinkPreview(Guid activityId, int previewId);

        Task<LinkPreview> GetActivityLinkPreviewAsync(Guid activityId);
        Task RemovePreviewRelationsAsync(Guid activityId);
        Task AddLinkPreviewAsync(Guid activityId, int previewId);
        Task UpdateLinkPreviewAsync(Guid activityId, int previewId);
    }
}
