using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;

namespace uIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewModel
    {
        public IEnumerable<int> MediaIds { get; set; }
        public int DisplayedImagesCount { get; set; }
        public Guid ActivityId { get; set; }
        public IActivityType ActivityType { get; set; }

        public LightboxGalleryPreviewModel()
        {
            MediaIds = Enumerable.Empty<int>();
        }
    }
}
