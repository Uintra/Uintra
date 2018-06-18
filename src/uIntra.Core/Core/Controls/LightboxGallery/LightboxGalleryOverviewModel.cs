using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryOverviewModel
    {
        public IEnumerable<LightboxGalleryItemViewModel> GalleryItems { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
    }
}