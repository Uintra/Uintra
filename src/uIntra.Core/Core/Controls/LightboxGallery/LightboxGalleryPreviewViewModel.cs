using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewViewModel
    {
        public IEnumerable<LightboxGalleryViewModel> Images { get; set; }
        public IEnumerable<LightboxGalleryViewModel> OtherFiles { get; set; }
        public string Url { get; set; }

        public LightboxGalleryPreviewViewModel()
        {
            Images = Enumerable.Empty<LightboxGalleryViewModel>();
            OtherFiles = Enumerable.Empty<LightboxGalleryViewModel>();
        }
    }
}
