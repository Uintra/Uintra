using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewModel
    {
        public IEnumerable<LightboxGalleryViewModel> Images { get; set; }

        public IEnumerable<LightboxGalleryViewModel> OtherFiles { get; set; }

        public LightboxGalleryPreviewModel()
        {
            Images = Enumerable.Empty<LightboxGalleryViewModel>();
            OtherFiles = Enumerable.Empty<LightboxGalleryViewModel>();
        }
    }
}
