using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Links;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewViewModel
    {
        public IEnumerable<LightboxGalleryItemViewModel> Images { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
        public IEnumerable<LightboxGalleryItemViewModel> OtherFiles { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
        public Guid ActivityId { get; set; }
        public IIntranetType ActivityType { get; set; }
        public IActivityLinks Links { get; set; }
    }
}
