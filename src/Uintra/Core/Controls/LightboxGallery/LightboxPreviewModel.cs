﻿using System.Collections.Generic;
using System.Linq;

namespace Uintra.Core.Controls.LightboxGallery
{
    public class LightboxPreviewModel
    {
        public IEnumerable<LightboxGalleryItemPreviewModel> Medias { get; set; } = Enumerable.Empty<LightboxGalleryItemPreviewModel>();
        public IEnumerable<LightboxGalleryItemPreviewModel> OtherFiles { get; set; } = Enumerable.Empty<LightboxGalleryItemPreviewModel>();
        public int HiddenImagesCount { get; set; }
        public int AdditionalImages { get; set; }
        public int FilesToDisplay { get; set; }
    }
}