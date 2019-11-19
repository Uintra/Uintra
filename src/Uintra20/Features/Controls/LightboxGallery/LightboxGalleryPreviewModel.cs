using System;
using System.Collections.Generic;
using System.Linq;

namespace Uintra20.Features.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewModel
    {
        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();
        public int DisplayedImagesCount { get; set; }
        public Guid ActivityId { get; set; }
        public Enum ActivityType { get; set; }
    }
}