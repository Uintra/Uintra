using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewViewModel
    {
        public IEnumerable<LightboxGalleryViewModel> Images { get; set; }
        public IEnumerable<LightboxGalleryViewModel> OtherFiles { get; set; }
        public Guid ActivityId { get; set; }
        public IIntranetType ActivityType { get; set; }

        public LightboxGalleryPreviewViewModel()
        {
            Images = Enumerable.Empty<LightboxGalleryViewModel>();
            OtherFiles = Enumerable.Empty<LightboxGalleryViewModel>();
        }
    }
}
