using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryOverviewModel
    {
        public IEnumerable<LightboxGalleryItemViewModel> Medias { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
        public IEnumerable<LightboxGalleryItemViewModel> OtherFiles { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
    }
}