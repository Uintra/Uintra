using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewModel
    {
        public IEnumerable<int> MediaIds { get; set; } = Enumerable.Empty<int>();
        public int DisplayedImagesCount { get; set; }
        public Guid ActivityId { get; set; }
        public IIntranetType ActivityType { get; set; }
    }
}
