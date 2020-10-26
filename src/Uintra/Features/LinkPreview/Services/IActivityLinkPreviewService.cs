using System;
using System.Threading.Tasks;
using Uintra.Features.LinkPreview.Models;

namespace Uintra.Features.LinkPreview.Services
{
    public interface IActivityLinkPreviewService
    {
	    LinkPreviewModel GetActivityLinkPreview(Guid activityId);
        void RemovePreviewRelations(Guid activityId);
        void AddLinkPreview(Guid activityId, int previewId);
        void UpdateLinkPreview(Guid activityId, int previewId);

        Task<LinkPreviewModel> GetActivityLinkPreviewAsync(Guid activityId);
        Task RemovePreviewRelationsAsync(Guid activityId);
        Task AddLinkPreviewAsync(Guid activityId, int previewId);
        Task UpdateLinkPreviewAsync(Guid activityId, int previewId);
    }
}