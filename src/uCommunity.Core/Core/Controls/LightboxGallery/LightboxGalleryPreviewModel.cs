using System;
using System.Collections.Generic;
using System.Linq;

namespace uCommunity.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewModel
    {
        public int MaxImagesCount { get; set; }
        public IEnumerable<int> MediaIds { get; set; }
        public string Url { get; set; }

        public LightboxGalleryPreviewModel()
        {
            MediaIds = Enumerable.Empty<int>();
        }
    }
}
