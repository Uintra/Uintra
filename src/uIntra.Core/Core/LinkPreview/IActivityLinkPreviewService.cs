using System;

namespace Uintra.Core.LinkPreview
{
    public interface IActivityLinkPreviewService
    {
        LinkPreview GetActivityLinkPreview(Guid activityId);
        void RemovePreviewRelations(Guid activityId);
        void AddLinkPreview(Guid activityId, int previewId);
        void UpdateLinkPreview(Guid commentId, int previewId);
    }
}
