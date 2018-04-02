using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Links;

namespace UIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewViewModel
    {
        public IEnumerable<LightboxGalleryItemViewModel> Medias { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
        public IEnumerable<LightboxGalleryItemViewModel> OtherFiles { get; set; } = Enumerable.Empty<LightboxGalleryItemViewModel>();
        public Guid ActivityId { get; set; }
        public Enum ActivityType { get; set; }
        public IActivityLinks Links { get; set; }
        public int HiddenImagesCount { get; set; }

    }
}
