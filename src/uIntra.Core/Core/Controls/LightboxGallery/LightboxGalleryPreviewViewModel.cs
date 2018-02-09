using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Links;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewViewModel
    {
        public IEnumerable<LightboxGalleryItemViewModel> Images { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
        public IEnumerable<LightboxGalleryItemViewModel> OtherFiles { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
        public Guid ActivityId { get; set; }
        public Enum ActivityType { get; set; }
        public IActivityLinks Links { get; set; }
    }
}
