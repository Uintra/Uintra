using System;
using System.Collections.Generic;
using Uintra.Core.Links;

namespace Uintra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewViewModel
    {
        public IList<LightboxGalleryItemViewModel> Images { get; set; }
        public IList<LightboxGalleryItemViewModel> OtherFiles { get; set; }
        public bool IsAttachedFileIconShown { get; set; }
        public int TotalFileCount { get; set; }
        public Guid ActivityId { get; set; }
        public Enum ActivityType { get; set; }
        public IActivityLinks Links { get; set; }
    }
}
