using System.Collections.Generic;
using System.Linq;

namespace uIntra.Core.Controls.LightboxGallery
{
    public class LightboxGalleryPreviewModel
    {
        public IEnumerable<int> MediaIds { get; set; }
        public string Url { get; set; }
        public int DisplayedImagesCount { get; set; }

        public LightboxGalleryPreviewModel()
        {
            MediaIds = Enumerable.Empty<int>();
        }
    }
}
