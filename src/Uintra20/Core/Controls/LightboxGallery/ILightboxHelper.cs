using System.Collections.Generic;

namespace Uintra20.Core.Controls.LightboxGallery
{
    public interface ILightboxHelper
    {
        void FillGalleryPreview(IHaveLightboxPreview model, IEnumerable<int> mediaIds);
        LightboxPreviewModel GetGalleryPreviewModel(IEnumerable<int> mediaIds);
    }
}
