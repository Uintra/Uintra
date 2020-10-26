using System.Collections.Generic;
using Uintra.Features.Media.Strategies.Preset;

namespace Uintra.Core.Controls.LightboxGallery
{
    public interface ILightboxHelper
    {
        void FillGalleryPreview(IHaveLightboxPreview model, IEnumerable<int> mediaIds);
        LightboxPreviewModel GetGalleryPreviewModel(IEnumerable<int> mediaIds, IPresetStrategy strategy);
    }
}
