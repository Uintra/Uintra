using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewModel
    {
        public IEnumerable<int> MediaIds { get; set; }
        public int DisplayedImagesCount { get; set; }
        public Guid ActivityId { get; set; }
        public IIntranetType ActivityType { get; set; }

        public LightboxGalleryPreviewModel()
        {
            MediaIds = Enumerable.Empty<int>();
        }
    }
}
